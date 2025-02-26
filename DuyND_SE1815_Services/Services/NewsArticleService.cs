using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Data.Repositories.Implementations;
using DuyND_SE1815_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DuyND_SE1815_Services.Services
{
    public class NewsArticleService
    {
        private readonly INewsArticleRepository _newsRepository;
        private readonly EmailService _emailService;
        public NewsArticleService(INewsArticleRepository newsRepository, EmailService emailService) 
        {
            _newsRepository = newsRepository;
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<List<NewsArticle>> GetAllNews()
        {
            return await _newsRepository.GetAllNewsWithDetails();
        }
        


        public async Task<NewsArticle?> GetNewsById(string id)
        {
            return await _newsRepository.GetNewsById(id);
        }

        public async Task<List<NewsArticle>> SearchNews(string keyword)
        {
            return await _newsRepository.SearchNews(keyword);
        }

        public async Task AddNews(NewsArticle news)
        {
            if (news == null) throw new ArgumentNullException(nameof(news));

          
            var lastNews = await _newsRepository.GetLastNewsArticle();

            int lastId = (lastNews != null && int.TryParse(lastNews.NewsArticleId, out int parsedId)) ? parsedId : 0;

            int newId = lastId + 1;
            news.NewsArticleId = newId.ToString();
            try
            {
                await _newsRepository.AddNews(news);
               
                Console.WriteLine($"News '{news.NewsTitle}' added to database successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add news to database: {ex.Message}");
            }

        }



        public async Task UpdateNews(NewsArticle news)
        {
            await _newsRepository.UpdateNews(news);
        }

        public async Task DeleteNews(string id)
        {
            await _newsRepository.DeleteNews(id);
        }
        public async Task<List<NewsArticle>> GetNewsByAuthorId(short? authorId) => await _newsRepository.GetNewsByAuthorId(authorId);
        public async Task<List<NewsArticle>> GetReportByDateRange(DateTime startDate, DateTime endDate) => await _newsRepository.GetReportByDateRange(startDate, endDate);
      
        public async Task<List<NewsArticle>> GetNewsHistoryByAuthorId(short authorId)
        {
            return await _newsRepository.GetNewsByAuthorId(authorId);
        }

        public async Task<List<SystemAccount>> GetAllLecturers()
        {
            return await _newsRepository.GetAllLecturers();
        }
      
        
    }
}
