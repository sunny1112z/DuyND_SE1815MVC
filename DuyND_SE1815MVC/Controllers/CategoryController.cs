using DuyND_SE1815_Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DuyND_SE1815MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly FunewsManagementContext _context;

        public CategoryController(FunewsManagementContext context)
        {
            _context = context;
        }

        // GET: Category
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // GET: Category/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // API: Lấy thông tin category theo ID (Dùng cho AJAX)
        [HttpGet]
        public IActionResult GetCategory(int id)
        {
            var category = _context.Categories
                .Select(c => new
                {
                    c.CategoryId,
                    c.CategoryName,
                    c.CategoryDesciption,
                    c.ParentCategoryId,
                    c.IsActive
                })
                .FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return Json(category);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories
                .Select(c => new { c.CategoryId, c.CategoryName })
                .ToListAsync();

            return Ok(categories);
        }

        // POST: Category/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ!");
            }

            if (category.ParentCategoryId != null)
            {
                var parentCategory = await _context.Categories.FindAsync(category.ParentCategoryId);
                if (parentCategory == null)
                {
                    return BadRequest("Parent Category không tồn tại!");
                }
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Danh mục đã được tạo thành công!" });
        }


        // GET: Category/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public IActionResult Edit([FromForm] Category category, [FromForm] string NewParentCategory)
        {
            Console.WriteLine($"Category ID: {category.CategoryId}");

            // Tìm category hiện tại từ cơ sở dữ liệu
            var existingCategory = _context.Categories.Find(category.CategoryId);
            if (existingCategory == null)
            {
                Console.WriteLine("Category not found!");
                return View(category); // Trả lại nếu không tìm thấy danh mục
            }

            // Kiểm tra xem ID có tồn tại trong DB không
            Console.WriteLine($"Existing Category Found: {existingCategory.CategoryId}, Name: {existingCategory.CategoryName}");

            // Cập nhật thông tin cho category
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.CategoryDesciption = category.CategoryDesciption;
            existingCategory.IsActive = category.IsActive;

            // Kiểm tra ParentCategoryId
            if (!string.IsNullOrEmpty(NewParentCategory))
            {
                existingCategory.ParentCategoryId = short.Parse(NewParentCategory);  // Chuyển đổi ID Parent từ string sang short
                Console.WriteLine($"Updated Parent Category ID: {existingCategory.ParentCategoryId}");
            }
            else
            {
                existingCategory.ParentCategoryId = null;  // Nếu không có parent category thì set null
            }

            // Ghi log trước khi lưu
            Console.WriteLine("Saving changes...");

            try
            {
                _context.SaveChanges();  // Lưu thay đổi vào DB
                Console.WriteLine("Changes saved successfully.");
                return RedirectToAction("Index");  // Điều hướng lại sau khi thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving changes: {ex.Message}");
                return View(category);  // Nếu có lỗi, trả về trang edit
            }
        }




        // GET: Category/Delete/5
        public IActionResult Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(short id)
        {
            try
            {
                var category = _context.Categories.Find(id);
                if (category == null)
                {
                    return NotFound();
                }

                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return View("CannotDelete");

            }

            return RedirectToAction(nameof(Index));
        }
    }
}
