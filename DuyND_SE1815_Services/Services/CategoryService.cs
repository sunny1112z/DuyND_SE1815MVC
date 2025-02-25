using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DuyND_SE1815_Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetCategoryById(short id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task AddCategory(Category category)
        {
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategory(Category category)
        {
            await _categoryRepository.Update(category);
        }

        public async Task DeleteCategory(short id)
        {
            await _categoryRepository.Delete(id);
        }
    }
}
