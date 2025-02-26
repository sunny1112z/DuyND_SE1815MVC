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
        public NewsArticleController(NewsArticleService newsService, EmailService emailService)
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
        public async Task<IActionResult> MyNewsHistory()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }


            var myNews = await _newsService.GetNewsHistoryByAuthorId((short)userId.Value);

            return View(myNews);
        }

        public async Task<IActionResult> Manage()
        {
            var newsArticles = await _newsService.GetAllNews();


            return View(newsArticles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var lecturers = await _newsService.GetAllLecturers();

            // Chuyển đổi danh sách giảng viên sang SelectListItem
            ViewBag.Lecturers = lecturers?.Select(l => new SelectListItem
            {
                Value = l.AccountId.ToString(),
                Text = l.AccountName
            }).ToList() ?? new List<SelectListItem>();  // Đảm bảo không bị null

            return View();
        }

        [HttpPost]

        [HttpPost]
        public async Task<IActionResult> Create([Bind("NewsTitle,Headline,NewsContent,NewsSource,CreatedDate,CategoryId,NewsStatus,CreatedById")] NewsArticle article)
        {
            // 🛠️ 1. Ghi log để kiểm tra dữ liệu đầu vào
            Console.WriteLine($"Received Create request: Title = {article.NewsTitle}, CreatedById = {article.CreatedById}");

            // 🛠️ 2. Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is NOT valid!");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                await LoadLecturers();
                return View(article);
            }

            // 🛠️ 3. Kiểm tra `CreatedById`
            if (article.CreatedById == null || article.CreatedById <= 0)
            {
                ModelState.AddModelError("CreatedById", "Please select a valid lecturer.");
                Console.WriteLine("ERROR: CreatedById is invalid or null!");
                await LoadLecturers();
                return View(article);
            }

            try
            {
                // 🛠️ 4. Gán `NewsArticleId` nếu nó chưa có giá trị
                article.NewsArticleId = Guid.NewGuid().ToString();

                // 🛠️ 5. Thêm bài viết vào Database
                await _newsService.AddNews(article);
                Console.WriteLine("News added successfully!");

                // 🛠️ 6. Gửi email thông báo
                string subject = $"New Article Published: {article.NewsTitle}";
                string body = $@"
        <h2>New Article Published</h2>
        <p><strong>Title:</strong> {article.NewsTitle}</p>
        <p><strong>Content:</strong> {article.NewsContent}</p>
        <p><a href='https://localhost:7221/NewsArticle/Details/{article.NewsArticleId}'>View Article</a></p>";

                await _emailService.SendEmailAsync("duyndhe170848@fpt.edu.vn", subject, body);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddNews: {ex.Message}");
                ModelState.AddModelError("", "Error saving the article.");
                await LoadLecturers();
                return View(article);
            }

            return RedirectToAction("Manage");
        }

        // 🛠️ Hàm Load giảng viên để tránh lặp code
        private async Task LoadLecturers()
        {
            var lecturers = await _newsService.GetAllLecturers();
            ViewData["Lecturers"] = lecturers?.Select(l => new SelectListItem
            {
                Value = l.AccountId.ToString(),
                Text = l.AccountName
            }).ToList() ?? new List<SelectListItem>();
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
