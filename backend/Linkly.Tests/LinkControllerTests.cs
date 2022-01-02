using System.Threading.Tasks;
using Linkly.Api.Controllers;
using Linkly.Api.Models;
using Linkly.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Linkly.Api.Services;
using System;

namespace Linkly.Tests
{
    public class LinkControllerTests
    {
        public const string MOCK_SLUG = "mSlug";
        public const string MOCK_URL = "https://youtube.com/watch?=ddskjalj";

        private readonly Mock<ILinkService> _service;
        private readonly Mock<ILogger<LinkController>> _logger;
        private readonly LinkController _linkController;

        public LinkControllerTests()
        {
            _service = new Mock<ILinkService>();
            _logger = new Mock<ILogger<LinkController>>();
            _linkController = new LinkController(_logger.Object, _service.Object);

            _service.Setup(s => s.CreateSlugAsync(MOCK_SLUG, MOCK_URL))
                .Returns(Task.FromResult(true));

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
