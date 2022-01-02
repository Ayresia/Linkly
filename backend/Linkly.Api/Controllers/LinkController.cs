using Linkly.Api.Models;
using Linkly.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Linkly.Api.Services;

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
                Link fetchedSlug = await _service.GetBySlugAsync(slug);
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

        [HttpPost("shorten")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ShortenUrlResponse>> ShortenUrl(ShortenUrlRequest req)
        {
            if (String.IsNullOrEmpty(req.Url))
                return BadRequest(new ErrorResponse(400, "Provide a valid URL."));

            if (!LinkService.IsUrlValid(req.Url))
                return BadRequest(new ErrorResponse(400, "Provide a valid URL."));

            if (!req.Url.StartsWith("https://") && !req.Url.StartsWith("http://"))
                req.Url = $"http://{req.Url}";

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
