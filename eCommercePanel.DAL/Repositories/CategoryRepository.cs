using eCommercePanel.DAL.Context;
using eCommercePanel.DAL.Entities;
using eCommercePanel.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.Repositories
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Category> _category;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
            _category = context.Set<Category>();
        }

        public async Task<List<Category>> GetAllAsync() => await _category.ToListAsync();
        public async Task<Category> GetByIdAsync(int id) => await _category.FindAsync(id);
        public async Task AddCategoryAsync (Category category) => await _category.AddAsync(category);
        public void Update(Category category) => _category.Update(category);
        public void DeleteCategory(Category category) => _category.Remove(category);
        public async Task SaveAsync() => await _context.SaveChangesAsync();

    }
}
