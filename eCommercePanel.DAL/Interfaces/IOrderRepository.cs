using eCommercePanel.DAL.Entities;

namespace eCommercePanel.DAL.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<List<Order>> GetOrdersByUserIdAsync(int userId);
    Task<Order> GetByIdAsync(int id);
    Task AddAsync(Order order);
    void Update(Order order);
    void Delete(Order order);
    Task SaveAsync();
}
