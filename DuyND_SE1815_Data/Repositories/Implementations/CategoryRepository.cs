using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuyND_SE1815_Data.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FunewsManagementContext _context;

        public CategoryRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(short id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(short id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));

            var category = await _context.Categories
                .Include(n => n.NewsArticles) 
                .FirstOrDefaultAsync(n => n.CategoryId == id);

            if (category == null)
                throw new KeyNotFoundException("Category not found!");

            if (category.NewsArticles.Any())
                throw new InvalidOperationException("Cannot delete category because it has associated News.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Category>> GetCategoriesByNameAsync(string name)
        {
            return await _context.Categories
                .Where(c => c.CategoryName.Contains(name))
                .ToListAsync();
        }
    }
}
