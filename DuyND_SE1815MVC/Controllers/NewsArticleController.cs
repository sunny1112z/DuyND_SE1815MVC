using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DuyND_SE1815MVC.Controllers
{
    public class NewsArticleController : Controller
    {
        private readonly NewsArticleService _newsService;

        public NewsArticleController(NewsArticleService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var newsArticles = (await _newsService.GetAllNews())
                                .Where(n => n.NewsStatus == true)
                                .ToList();

            return View(newsArticles);
        }

        public async Task<IActionResult> Manage()
        {
            var newsArticles = await _newsService.GetAllNews();
                                

            return View(newsArticles);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(NewsArticle article)
        {
            if (ModelState.IsValid)
            {
                await _newsService.AddNews(article);
                return RedirectToAction("Index");
            }
            return View(article);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var article = await _newsService.GetNewsById(id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NewsArticle article)
        {
            if (ModelState.IsValid)
            {
                await _newsService.UpdateNews(article);
                return RedirectToAction("Index");
            }
            return View(article);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var article = await _newsService.GetNewsById(id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _newsService.DeleteNews(id);
            return RedirectToAction("Index");
        }
    }
}
 