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

public class OrderItemRepository : IOrderItemRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<OrderItem> _orderItem;

    public OrderItemRepository(AppDbContext context)
    {
        _context = context;
        _orderItem = context.Set<OrderItem>();
    }
    // ✔ Tek bir OrderItem getir
    public async Task<OrderItem> GetByIdAsync(int id)
    {
        return await _orderItem
            .Include(oi => oi.Product)
            .FirstOrDefaultAsync(oi => oi.Id == id);
    }
    // ✔ Tüm OrderItem'ları getir
    public async Task<List<OrderItem>> GetAllAsync()
    {
        return await _orderItem
            .Include(oi => oi.Product)
            .ToListAsync();
    }
    // ✔ Yeni OrderItem ekle
    public async Task AddAsync(OrderItem orderItem)
    {
        await _orderItem.AddAsync(orderItem);
        await _context.SaveChangesAsync();
    }
    // ✔ Güncelle
    public async Task UpdateAsync(OrderItem orderItem)
    {
        _orderItem.Update(orderItem);
        await _context.SaveChangesAsync();
    }
    // ✔ Sil (ID ile)
    public async Task DeleteByIdAsync(int id)
    {
        var item = await _orderItem.FindAsync(id);
        if (item != null)
        {
            _orderItem.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
    // ✔ Belirli bir siparişin ürünleri
    public async Task<List<OrderItem>> GetByOrderIdAsync(int orderId)
    {
        return await _orderItem
            .Include(oi => oi.Product)
            .Where(oi => oi.OrderId == orderId)
            .ToListAsync();
    }
    // ✔ Belirli bir ürüne ait tüm sipariş kalemleri
    public async Task<List<OrderItem>> GetByProductIdAsync(int productId)
    {
        return await _orderItem
            .Include(oi => oi.Product)
            .Where(oi => oi.ProductId == productId)
            .ToListAsync();
    }
    // ✔ Belirli sipariş ve ürün eşleşmesine göre sipariş kalemi
    public async Task<OrderItem> GetByOrderIdAndProductIdAsync(int orderId, int productId)
    {
        return await _orderItem
            .Include(oi => oi.Product)
            .FirstOrDefaultAsync(oi => oi.OrderId == orderId && oi.ProductId == productId);
    }
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }


}
