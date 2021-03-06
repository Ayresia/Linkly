using System.Text.RegularExpressions;
using Linkly.Api.Interfaces;
using Linkly.Api.Models;
using StackExchange.Redis;

namespace Linkly.Api.Services
{
    public class LinkService : ILinkService
    {
        private LinkContext _context;
        private IConnectionMultiplexer _redis;

        public LinkService(LinkContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
        }

        public async virtual Task<Link> GetBySlugAsync(string slug)
        {
            var redisDB = _redis.GetDatabase();
            var redisRes = await redisDB.StringGetAsync($"links:{slug}");

            if (redisRes.IsNullOrEmpty)
            {
                Link? fetchedLink = await _context.Links.FindAsync(slug);

                if (fetchedLink != null)
                    await redisDB.StringSetAsync($"links:{fetchedLink.Slug}", fetchedLink.Url, TimeSpan.FromMinutes(5));
                    return fetchedLink!;
            }


            return new Link()
            {
                Slug = slug,
                Url = redisRes.ToString()
            };
        }

        public virtual Link? GetByUrl(string url)
        {
            Link? fetchedLink = _context.Links
                .Where(l => l.Url == url)
                .FirstOrDefault();

            if (fetchedLink == null)
                return null;

            return new Link()
            {
                Slug = fetchedLink.Slug,
                Url = fetchedLink.Url
            };
        }

        public string SanitizeUrl(string url)
        {
            if (!url.StartsWith("https://") && !url.StartsWith("http://"))
                url = $"https://{url}";

            if (url.StartsWith("http://"))
                url = url.Replace("http://", "https://");

            if (url.Contains("www."))
                url = url.Replace("www.", string.Empty);

            return url;
        }

        public bool IsUrlValid(string url)
        {
            string pattern = @"^(https?:\/\/)?([\w\d-_]+)\.([\w\d-_\.]+)\/?\??([^#\n\r]*)?#?([^\n\r]*)";

            Regex rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rgx.IsMatch(url);
        }

        public async virtual Task CreateSlugAsync(string slug, string url)
        {
            await _context.Links.AddAsync(new Link
                {
                    Slug = slug,
                    Url = url
                }
            );

            await _context.SaveChangesAsync();

            var redisDB = _redis.GetDatabase();
            await redisDB.StringSetAsync($"links:{slug}", url, TimeSpan.FromMinutes(5));
        }

        public async virtual Task<string> CreateUniqueSlugAsync(string url)
        {
            string generatedSlug = GenerateSlug();
            var queryResult = _context.Links
                .Where(l => l.Slug == generatedSlug)
                .FirstOrDefault();

            if (queryResult != null)
                return await CreateUniqueSlugAsync(url);

            await CreateSlugAsync(generatedSlug, url);
            return generatedSlug;
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
