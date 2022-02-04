using Linkly.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Linkly.Api.Interfaces;

namespace Linkly.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly ILogger<LinkController> _logger;
        private ILinkService _service;

        public LinkController(ILogger<LinkController> logger, ILinkService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("info/{slug}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SlugInfoResponse>> SlugInfo(string slug)
        {
            try 
            {
                if (slug.Length != 5) {
                    _logger.LogInformation($"SlugInfo - {slug} is too short.");
                    return BadRequest(new ErrorResponse(400, "Provide a valid slug."));
                }

                Link fetchedSlug = await _service.GetBySlugAsync(slug);
                _logger.LogInformation($"SlugInfo - {slug} {fetchedSlug.Url}");

                return Ok(
                    new SlugInfoResponse
                    {
                        Url = fetchedSlug.Url
                    }
                );
            }
            catch (NullReferenceException)
            {
                _logger.LogError($"SlugInfo - {slug} does not exist.");
                return BadRequest(new ErrorResponse(400, "The slug entered does not exist."));
            }
            catch
            {
                _logger.LogError("SlugInfo - Internal server error.");

                return StatusCode
                (
                    StatusCodes.Status500InternalServerError,
                    new ErrorResponse(500, "An error has occured, please try again later.")
                );
            }
        }

        [HttpPost("shorten")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ShortenUrlResponse>> ShortenUrl(ShortenUrlRequest req)
        {
            if (String.IsNullOrEmpty(req.Url))
            {
                _logger.LogError("ShortenUrl - No url has been provided.");
                return BadRequest(new ErrorResponse(400, "Provide a valid URL."));
            }

            if (req.Url.Length > 2048)
            {
                _logger.LogError("ShortenUrl - URL too large.");
                return BadRequest(new ErrorResponse(400, "The URL length must be less than 2048."));
            }

            if (!_service.IsUrlValid(req.Url)) {
                _logger.LogError("ShortenUrl - URL is not valid.");
                return BadRequest(new ErrorResponse(400, "Provide a valid URL."));
            }

            string sanitizedUrl = _service.SanitizeUrl(req.Url);
            string generatedSlug;

            try 
            {
                var fetchedLink = _service.GetByUrl(sanitizedUrl);

                if (fetchedLink == null) {
                    generatedSlug = await _service.CreateUniqueSlugAsync(sanitizedUrl);
                } else {
                    generatedSlug = fetchedLink!.Slug;
                }

                _logger.LogInformation($"ShortenUrl - Slug {generatedSlug} has been generated.");

                return StatusCode
                (
                    StatusCodes.Status201Created,
                    new ShortenUrlResponse
                    { 
                        Slug = generatedSlug
                    }
                );
            }
            catch
            {
                _logger.LogError("ShortenUrl - Internal server error.");

                return StatusCode
                (
                    StatusCodes.Status500InternalServerError,
                    new ErrorResponse(500, "An error has occured, please try again later.")
                );
            }
        }
    }
}
