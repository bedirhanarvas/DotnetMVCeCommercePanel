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

internal class OrderRepository:IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Order> _orders;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
        _orders = context.Set<Order>();
    }

    public async Task<List<Order>> GetAllAsync() => await _orders.ToListAsync();

    public async Task<Order> GetByIdAsync(int id) => await _orders.FindAsync(id);

    public async Task AddAsync(Order order) => await _orders.AddAsync(order);

    public void Update(Order order) => _orders.Update(order);

    public void Delete(Order order) => _orders.Remove(order);

    public async Task SaveAsync() => await _context.SaveChangesAsync();

    public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await _orders
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }
}
