using Linkly.Models;
using Linkly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Linkly.Controllers
{
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly ILogger<LinkController> _logger;
        private LinkService _service;

        public LinkController(ILogger<LinkController> logger, LinkService service)
        {
            _logger = logger;
            _service = service;
        }

       
        [HttpGet("/info/{slug}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SlugInfoResponse>> SlugInfo(string slug)
        {
            try 
            {
                Link fetchedSlug = await _service.GetBySlug(slug);
                return Ok(
                    new SlugInfoResponse
                    {
                        Url = fetchedSlug.Url
                    }
                );
            }
            catch (NullReferenceException)
            {
                return BadRequest(new ErrorResponse(400, "The slug entered does not exist."));
            }
            catch
            {
                return StatusCode
                (
                    StatusCodes.Status500InternalServerError,
                    new ErrorResponse(500, "An error has occured, please try again later.")
                );
            }
        }

        [HttpPost("api/shorten")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ShortenUrlResponse>> ShortenUrl(ShortenUrlRequest req)
        {
            if (String.IsNullOrEmpty(req.Url))
                return BadRequest(new ErrorResponse(400, "Provide a valid URL."));

            if (!req.Url.StartsWith("https://") && !req.Url.StartsWith("http://"))
                req.Url = $"http://{req.Url}";

            if (!Uri.IsWellFormedUriString(req.Url, UriKind.Absolute))
                return BadRequest(new ErrorResponse(400, "Provide a valid URL."));

            Uri uri = new Uri(req.Url);
            string sanitizedUrl = uri.GetLeftPart(UriPartial.Path);
            string generatedSlug = _service.GenerateSlug();

            try 
            {
                await _service.CreateSlugAsync(generatedSlug, sanitizedUrl);
            }
            catch
            {
                return StatusCode
                (
                    StatusCodes.Status500InternalServerError,
                    new ErrorResponse(500, "An error has occured, please try again later.")
                );
            }

            return StatusCode
            (
                StatusCodes.Status201Created,
                new ShortenUrlResponse
                { 
                    Slug = generatedSlug 
                }
            );
        }
    }
}
