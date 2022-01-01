using Linkly.Models;

namespace Linkly.Services
{
    public class LinkService
    {
        private LinkContext _context;

        public LinkService(LinkContext context)
        {
            _context = context;
        }

        public async Task<Link> GetBySlug(string slug)
        {
            Link fetchedLink = await _context.Links.FindAsync(slug);
            return fetchedLink;
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
