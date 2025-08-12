using eCommercePanel.BLL.Results;
using eCommercePanel.DAL.DTOs.AddressDTOs.Requests;
using eCommercePanel.DAL.DTOs.OrderDTOs.Requests;
using eCommercePanel.DAL.DTOs.OrderDTOs.Responses;

namespace eCommercePanel.BLL.Services
{
    public interface IOrderService
    {
        Task<DataResult<List<AllOrdersDto>>> GetAllOrders();
        Task<DataResult<OrderDetailDto>> GetOrderDetail(int id);
        Task<Result> AddAsync(OrderCreateDto orderCreateDto);
        Task<Result> UpdateAsync(OrderUpdateDto orderUpdateDto);
        Task<Result> DeleteOrderAsync(int orderId);
        Task<DataResult<List<OrderDetailDto>>> GetOrdersByUserIdAsync(int userId);
    }
}
