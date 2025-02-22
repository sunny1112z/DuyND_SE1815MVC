using DuyND_SE1815_Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DuyND_SE1815_Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(short id);
        Task AddAsync(Category category);
        Task Update(Category category);
        Task Delete(Category category);
        Task<IEnumerable<Category>> GetCategoriesByNameAsync(string name);
    }
}
