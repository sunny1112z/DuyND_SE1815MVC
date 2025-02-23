using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DuyND_SE1815_Services.Services
{
    public class NewsArticleService
    {
        private readonly INewsArticleRepository _newsRepository;

        public NewsArticleService(INewsArticleRepository newsRepository)
        {
            _newsRepository = newsRepository;
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
            await _newsRepository.AddNews(news);
        }


        public async Task UpdateNews(NewsArticle news)
        {
            await _newsRepository.UpdateNews(news);
        }

        public async Task DeleteNews(string id)
        {
            await _newsRepository.DeleteNews(id);
        }
    }
}
