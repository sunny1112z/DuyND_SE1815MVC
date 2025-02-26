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
            return await _context.NewsArticles. Include(n => n.Category)
        .Include(n => n.CreatedBy)
                .Where(n => n.NewsTitle.Contains(keyword) || n.NewsContent.Contains(keyword))
                .ToListAsync();
        }

        public async Task AddNews(NewsArticle news)
        {
            await _context.NewsArticles.AddAsync(news);
            await _context.SaveChangesAsync();
        }
        public async Task<NewsArticle?> GetLastNewsArticle()
        {
            var newsArticles = await _context.NewsArticles.ToListAsync();

            return newsArticles
                .OrderByDescending(n => int.TryParse(n.NewsArticleId, out int id) ? id : 0)
                .FirstOrDefault();
        }


        public async Task UpdateNews(NewsArticle news)
        {
            _context.NewsArticles.Update(news);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNews(string id)
        {
         
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            var news = await _context.NewsArticles
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);

            if (news == null) throw new KeyNotFoundException("News article not found!");

            if (news.Tags.Any())
            {
                throw new InvalidOperationException("Cannot delete news article because it has associated tags.");
            }

            _context.NewsArticles.Remove(news);
            await _context.SaveChangesAsync();
        }
        public async Task<List<NewsArticle>> GetNewsByAuthorId(short? authorId)
        {
            return await _context.NewsArticles
                .Include(c => c.Category)
                .Where(n => n.CreatedById == authorId)
                .ToListAsync();
        }
        public async Task<List<NewsArticle>> GetNewsHistoryByAuthorId(short authorId)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedById == authorId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }
        public async Task<List<SystemAccount>> GetAllLecturers()
        {
            return await _context.SystemAccounts
                .Where(a => a.AccountRole == 2) 
                .ToListAsync();
        }
        public async Task<List<NewsArticle>> GetReportByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }
    }
}
