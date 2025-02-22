using DuyND_SE1815_Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DuyND_SE1815_Data.Repositories.Interfaces
{
    public interface INewsArticleRepository
    {
        Task<List<NewsArticle>> GetAllNews();
        Task<NewsArticle?> GetNewsById(string id);
        Task<List<NewsArticle>> SearchNews(string keyword);
        Task AddNews(NewsArticle news);
        Task UpdateNews(NewsArticle news);
        Task DeleteNews(string id);
    }
}
