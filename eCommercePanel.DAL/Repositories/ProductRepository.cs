using eCommercePanel.DAL.Context;
using eCommercePanel.DAL.Entities;
using eCommercePanel.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.Repositories;

public class ProductRepository:IProductRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Product> _products;

    public ProductRepository (AppDbContext context)
    {
        _context = context;
        _products = context.Set<Product>();
    }
    public async Task<List<Product>> GetAllAsync() => await _products.ToListAsync();
    public async Task<Product> GetByIdAsync(int id) => await _products.FindAsync(id);
    public async Task<Product> GetByNameAsync(string name) =>
    await _products.FirstOrDefaultAsync(p => p.ProductName == name);

    public async Task AddAsync(Product product) => await _products.AddAsync(product);
    public async Task Update(Product product) => _products.Update(product);
    public void Delete(Product product) => _products.Remove(product);
    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
