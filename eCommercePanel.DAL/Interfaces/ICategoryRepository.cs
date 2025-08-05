using eCommercePanel.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddCategoryAsync (Category category);
        void Update(Category category);
        void DeleteCategory (Category category);
        Task SaveAsync();

    }
}
