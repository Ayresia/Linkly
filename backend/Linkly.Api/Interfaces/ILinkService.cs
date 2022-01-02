using Linkly.Api.Models;

namespace Linkly.Api.Interfaces
{
    public interface ILinkService
    {
        Task<Link> GetBySlugAsync(string slug);
        Task CreateSlugAsync(string slug, string url);
        string GenerateSlug();
    }
}
