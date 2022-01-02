using System.Text.RegularExpressions;
using Linkly.Api.Interfaces;
using Linkly.Api.Models;

namespace Linkly.Api.Services
{
    public class LinkService : ILinkService
    {
        private LinkContext _context;

        public LinkService(LinkContext context)
        {
            _context = context;
        }

        public async Task<Link> GetBySlugAsync(string slug)
        {
            Link fetchedLink = await _context.Links.FindAsync(slug);
            return fetchedLink;
        }

        public bool IsUrlValid(string url)
        {
            string pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";

            Regex rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rgx.IsMatch(url);
        }

        public async Task CreateSlugAsync(string slug, string url)
        {
            await _context.Links.AddAsync(new Link
                {
                    Slug = slug,
                    Url = url
                }
            );

            await _context.SaveChangesAsync();
        }

        public string GenerateSlug()
        {
            Random random = new Random();
            string charSet = "abcdefghijklmnopqrstuvxyz0123456789";

            return new String(Enumerable.Range(0, 5)
                .Select(i => charSet[random.Next(0, charSet.Length)])
                .ToArray()
            );
        }
    }
}
