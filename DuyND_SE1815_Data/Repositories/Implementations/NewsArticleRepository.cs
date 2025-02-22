using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuyND_SE1815_Data.Repositories.Implementations
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FunewsManagementContext _context;

        public NewsArticleRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<List<NewsArticle>> GetAllNewsWithDetails()
        {
            return await _context.NewsArticles
                                 .Include(n => n.CreatedBy)
                                 .Include(n => n.Category)
                                 .ToListAsync();
        }

        public async Task<NewsArticle?> GetNewsById(string id)
        {
            return await _context.NewsArticles
                                 .Include(n => n.CreatedBy)
                                 .Include(n => n.Category)
                                 .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }

        public async Task<List<NewsArticle>> SearchNews(string keyword)
        {
            return await _context.NewsArticles
                .Where(n => n.NewsTitle.Contains(keyword) || n.NewsContent.Contains(keyword))
                .ToListAsync();
        }

        public async Task AddNews(NewsArticle news)
        {
            await _context.NewsArticles.AddAsync(news);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNews(NewsArticle news)
        {
            _context.NewsArticles.Update(news);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNews(string id)
        {
            var news = await _context.NewsArticles.FindAsync(id);
            if (news != null)
            {
                _context.NewsArticles.Remove(news);
                await _context.SaveChangesAsync();
            }
        }
    }
}
