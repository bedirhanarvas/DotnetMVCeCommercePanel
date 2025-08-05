using eCommercePanel.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task<Product> GetByNameAsync(string name);
    Task AddAsync(Product product);
    Task Update(Product product);
    void Delete(Product product);
    Task SaveAsync();
}
