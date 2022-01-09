using System.Threading.Tasks;
using Linkly.Api.Controllers;
using Linkly.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Linkly.Api.Services;
using System;
using StackExchange.Redis;

namespace Linkly.Tests
{
    public class LinkControllerTests
    {
        public const string MOCK_SLUG = "mslug";
        public const string MOCK_URL = "https://examplesite.com";

        private readonly LinkContext _context;
        private readonly Mock<IConnectionMultiplexer> _redis;
        private readonly Mock<IDatabase> _redisDatabase;
        private readonly Mock<LinkService> _service;
        private readonly Mock<ILogger<LinkController>> _logger;
        private readonly LinkController _linkController;

        public LinkControllerTests()
        {
            _context = new LinkContext();
            _redis = new Mock<IConnectionMultiplexer>();
            _redisDatabase = new Mock<IDatabase>();
            _service = new Mock<LinkService>(_context, _redis.Object);
            _logger = new Mock<ILogger<LinkController>>();
            _linkController = new LinkController(_logger.Object, _service.Object);

            _redis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_redisDatabase.Object);
            
            _redisDatabase.Setup(rd => rd.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(
                    (RedisKey key, CommandFlags flags) =>
                    {
                        if (key != MOCK_SLUG)
                            return new RedisValue(null);

                        return MOCK_SLUG;
                    }
                );

            _service.Setup(s => s.CreateSlugAsync(MOCK_SLUG, MOCK_URL))
                .Returns(Task.FromResult(true));

            _service.Setup(s => s.CreateUniqueSlugAsync(MOCK_URL))
                .Returns(Task.FromResult(MOCK_SLUG));

            var mockSlugInfo = new Link
            {
                Slug = MOCK_SLUG,
                Url = MOCK_URL
            };

            _service.Setup(s => s.GetBySlugAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(mockSlugInfo))
                .Callback(
                    (string slug) => 
                    {
                        if (slug != MOCK_SLUG)
                            throw new NullReferenceException();
                    }
                );
        }

        [Fact]
        public async Task ShortenUrl_ValidUrl_ReturnsSlug()
        {

            var mockRequest = new ShortenUrlRequest
            {
                Url = MOCK_URL,
            };

            var result = await _linkController.ShortenUrl(mockRequest);
            var objectResult = Assert.IsType<ObjectResult>(result.Result);

            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        }

        [Fact]
        public async Task ShortenUrl_InvalidUrl_ReturnsBadRequest()
        {
            var mockRequest = new ShortenUrlRequest
            {
                Url = "example"
            };

            var result = await _linkController.ShortenUrl(mockRequest);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task SlugInfo_ValidSlug_ReturnSlugInfo()
        {
            var result = await _linkController.SlugInfo(MOCK_SLUG);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task SlugInfo_InvalidSlug_ReturnBadRequest()
        {
            var result = await _linkController.SlugInfo("example");
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
