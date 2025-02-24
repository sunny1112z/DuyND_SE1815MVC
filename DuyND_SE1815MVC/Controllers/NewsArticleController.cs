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
        private readonly EmailService _emailService;
        public NewsArticleController(NewsArticleService newsService , EmailService emailService)
        {
            _newsService = newsService;
            _emailService = emailService;
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

            
                string subject = "New Article Published: " + article.NewsTitle;
                string body = $@"
                  <h2>New Article Published</h2>
                  <p><strong>Title:</strong> {article.NewsTitle}</p>
                  <p><strong>Content:</strong> {article.NewsContent}</p>
                  <p><a href='https://localhost:7221/NewsArticle/Details/{article.NewsArticleId}'>View Article</a></p>";

                try
                {
                    await _emailService.SendEmailAsync("duyndhe170848@fpt.edu.vn", subject, body);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                }

                return RedirectToAction("Manage");
            }
            return View(article);
        }


        public async Task<IActionResult> Edit(string id)
        {
            var article = await _newsService.GetNewsById(id);
            if (article == null) return NotFound();

            return View(article);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var article = await _newsService.GetNewsById(id);
            if (article == null) return NotFound();

            return View(article);
        }


        [HttpGet]
        public async Task<IActionResult> SearchNews(string key)
        {
            var articles = await _newsService.SearchNews(key);

            if (articles == null || !articles.Any())
            {
                ModelState.AddModelError("", "Không tìm thấy bài viết nào!");
                return View(new List<NewsArticle>()); 
            }

            return View(articles);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(NewsArticle article)
        {
            if (ModelState.IsValid)
            {
                await _newsService.UpdateNews(article);
                return RedirectToAction("Manage");
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
            try
            {

                await _newsService.DeleteNews(id);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return View("CannotDelete");

            }

            return RedirectToAction("Index");
        }
        [HttpGet("Reports")]
        public IActionResult Reports()
        {
            return View();
        }
        [HttpPost("Reports")]
        public async Task<IActionResult> Reports(DateTime startDate, DateTime endDate)
        {
            var reportData = await _newsService.GetReportByDateRange(startDate, endDate);
            return View(reportData);
        }
    }
}
