using System.Threading.Tasks;
using Moq;
using Xunit;
using Linkly.Api.Services;
using Linkly.Api.Models;
using System;

namespace Linkly.Tests
{
    public class LinkServiceTests 
    {
        public const string MOCK_SLUG = "mslug";
        public const string MOCK_URL = "https://examplesite.com";

        private readonly Mock<LinkContext> _context;
        private readonly LinkService _service;

        public LinkServiceTests()
        {
            _context = new Mock<LinkContext>();
            _service = new LinkService(_context.Object);

            var mockSlugInfo = new Link
            {
                Slug = MOCK_SLUG,
                Url = MOCK_URL
            };

            _context.Setup(c => c.Links.FindAsync(It.IsAny<String>()))
                .ReturnsAsync(
                    (dynamic link) =>
                    {
                        if (link[0] != MOCK_SLUG)
                            return null;

                        return mockSlugInfo;
                    }
                );
        }

        [Fact]
        public async Task GetBySlugAsync_ValidSlug_ReturnLink()
        {
            var result = await _service.GetBySlugAsync(MOCK_SLUG);
            Assert.IsType<Link>(result);
        }

        [Fact]
        public async Task GetBySlugAsync_InvalidSlug_ReturnNull()
        {
            var result = await _service.GetBySlugAsync("example");
            Assert.IsNotType<Link>(result);
        }

        [Fact]
        public async Task IsUrlValid_ValidUrl_ReturnTrue()
        {
            var result = _service.IsUrlValid(MOCK_URL);
            Assert.True(result);
        }

        [Fact]
        public async Task IsUrlValid_InvalidUrl_ReturnFalse()
        {
            var result = _service.IsUrlValid("example site.com");
            Assert.False(result);
        }
    }
}
