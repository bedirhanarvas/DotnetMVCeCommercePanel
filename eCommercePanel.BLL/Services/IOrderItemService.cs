using eCommercePanel.BLL.Results;
using eCommercePanel.DAL.DTOs.OrderItemDTOs.Requests;
using eCommercePanel.DAL.DTOs.OrderItemDTOs.Response;

namespace eCommercePanel.BLL.Services;

public interface IOrderItemService
{
    Task<DataResult<List<GetAllOrderItemsDto>>> GetAllAsync();
    Task<DataResult<GetAllOrderItemsDto>> GetByIdAsync(int Id);
    Task<DataResult<List<GetAllOrderItemsDto>>> GetByOrderIdAsync(int orderId);
    Task<Result> AddAsync(OrderItemCreateDto orderItemCreateDto);
    Task<Result> DeleteOrderItemAsync(int Id);
    Task<DataResult<decimal>> GetTotalOrderPriceAsync(int Id);
    Task<DataResult<List<GetAllOrderItemsDto>>> GetByUserIdAsync(int userId);
    Task<DataResult<GetOrderItemDetailDto>> GetOrderItemDetailsAsync(int Id);
}
