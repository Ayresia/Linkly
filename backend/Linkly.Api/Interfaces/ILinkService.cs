using Linkly.Api.Models;

namespace Linkly.Api.Interfaces
{
    public interface ILinkService
    {
        Task<Link> GetBySlugAsync(string slug);
        bool IsUrlValid(string url);
        Task CreateSlugAsync(string slug, string url);
        Task<string> CreateUniqueSlugAsync(string url);
        string GenerateSlug();
    }
}
