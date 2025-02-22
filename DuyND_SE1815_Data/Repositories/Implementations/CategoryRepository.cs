﻿using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task Delete(Category category)
        {
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
