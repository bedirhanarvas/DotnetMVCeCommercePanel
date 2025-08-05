using eCommercePanel.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.Interfaces;

public interface IOrderItemRepository
{
    Task<OrderItem> GetByIdAsync(int id);
    Task<List<OrderItem>> GetAllAsync();
    Task AddAsync(OrderItem orderItem);
    Task UpdateAsync(OrderItem orderItem);
    Task DeleteByIdAsync(int id);
    Task<List<OrderItem>> GetByOrderIdAsync(int orderId);
    Task<List<OrderItem>> GetByProductIdAsync(int productId);
    Task<OrderItem> GetByOrderIdAndProductIdAsync(int orderId, int productId);
    Task SaveAsync();
}
