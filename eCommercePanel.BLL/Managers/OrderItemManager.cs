using eCommercePanel.BLL.Results;
using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.OrderItemDTOs.Requests;
using eCommercePanel.DAL.DTOs.OrderItemDTOs.Response;
using eCommercePanel.DAL.Entities;
using eCommercePanel.DAL.Interfaces;

namespace eCommercePanel.BLL.Managers;

public class OrderItemManager : IOrderItemService
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderItemManager(IOrderItemRepository orderItemRepository, IProductRepository productRepository, IOrderRepository orderRepository)
    {
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public async Task<Result> AddAsync(OrderItemCreateDto orderItemCreateDto)
    {
        var order = await _orderRepository.GetByIdAsync(orderItemCreateDto.OrderId);
        var product = await _productRepository.GetByIdAsync(orderItemCreateDto.ProductId);

        if (order == null)
        {
           return new ErrorResult("Sipariş bulunamadı.");
        }
        if (product == null)
        {
            return new ErrorResult("Ürün bulunamadı.");
        }
        var orderItem = new OrderItem
        {
            OrderId = orderItemCreateDto.OrderId,
            ProductId = orderItemCreateDto.ProductId,
            Quantity = orderItemCreateDto.Quantity,
            UnitPrice = orderItemCreateDto.UnitPrice
        };

        await _orderItemRepository.AddAsync(orderItem);
        await _orderItemRepository.SaveAsync();

        return new SuccessResult("Ürün sepete eklendi.");
    }

    public async Task<Result> DeleteOrderItemAsync(int Id)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(Id);

        if (orderItem == null)
        {

        }
        var order = await _orderRepository.GetByIdAsync(orderItem.OrderId);
        if (order.Status == "Completed" || order.Status == "Shipped")
        {
            // Sipariş tamamlanmışsa veya gönderildiyse, öğe silinemez
            return new ErrorResult("Kargolanan veya teslim edilen üürnler silinemez.");
            
        }
        await _orderItemRepository.DeleteByIdAsync(orderItem.Id);

        return new SuccessResult("Ürün başarıyla silindi.");
    }

    public async Task<DataResult<List<GetAllOrderItemsDto>>> GetAllAsync()
    {
        var orderItems = await _orderItemRepository.GetAllAsync();

        if(orderItems == null || !orderItems.Any())
        {
            return new ErrorDataResult<List<GetAllOrderItemsDto>>(null,"Hiçbir sipariş ürünü yok.");
        }

        var orderItemsDto = orderItems.Select(orderItem => new GetAllOrderItemsDto
        {
            Id = orderItem.Id,
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            TotalPrice = orderItem.Quantity * orderItem.UnitPrice,
        }).ToList();

        return new SuccessDataResult<List<GetAllOrderItemsDto>>(orderItemsDto,"Siparişte olan ürünler listelendi.");
    }

    public async Task<DataResult<List<GetAllOrderItemsDto>>> GetByUserIdAsync(int userId)
    {
        var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
        if(orders == null || orders.Count == 0)
        {
            return new ErrorDataResult<List<GetAllOrderItemsDto>>(null,"Sipariş bulunamadı.");
        }

        var allOrderItems = new List<OrderItem>();
        foreach (var order in orders)
        {
            var orderItems = await _orderItemRepository.GetByOrderIdAsync(order.Id);
            if (orderItems != null)
            {
                allOrderItems.AddRange(orderItems);
            }
        }

        var orderItemsDto = allOrderItems.Select(oi => new GetAllOrderItemsDto
        {
            Id = oi.Id,
            OrderId = oi.OrderId,
            ProductId = oi.ProductId,
            ProductName = oi.Product.ProductName,
            Quantity = oi.Quantity,
            UnitPrice = oi.UnitPrice,
            TotalPrice = oi.Quantity * oi.UnitPrice
        }).ToList();

        return new SuccessDataResult<List<GetAllOrderItemsDto>>(orderItemsDto, "Başarıyla listelendi.");
    }

    public async Task<DataResult<GetAllOrderItemsDto>> GetByIdAsync(int id)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(id);
        if(orderItem == null)
        {
            return new ErrorDataResult<GetAllOrderItemsDto> (null,"Sipariş ürünü bulunamdaı.");
        }

        var orderItemDto = new GetAllOrderItemsDto
        {
            Id = orderItem.Id,
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            ProductName = orderItem.Product.ProductName,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            TotalPrice = orderItem.Order.TotalAmount,
        };

        return new SuccessDataResult<GetAllOrderItemsDto>(orderItemDto,"Sepetiniz başarıyla getirildi.");
    }

    public async Task<DataResult<List<GetAllOrderItemsDto>>> GetByOrderIdAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if(order == null)
        {
            return new ErrorDataResult<List<GetAllOrderItemsDto>>(null,"Bu sipariş bulunamadı.");
        }
        var allOrderItems = new List<OrderItem>();
        foreach(var item in order.OrderItems)
        {
            allOrderItems.Add(item);
        }

        var orderItemsDto = allOrderItems.Select(oi => new GetAllOrderItemsDto
        {
            Id = oi.Id,
            OrderId = oi.OrderId,
            ProductId = oi.ProductId,
            ProductName = oi.Product.ProductName,
            Quantity = oi.Quantity,
            UnitPrice = oi.UnitPrice,
            TotalPrice = oi.Quantity * oi.UnitPrice
        }).ToList();

        return new SuccessDataResult<List<GetAllOrderItemsDto>>(orderItemsDto, "Başarıyla listelendi.");
    }

    public async Task<DataResult<GetOrderItemDetailDto>> GetOrderItemDetailsAsync(int Id)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(Id);
        if(orderItem == null)
        {
            return new ErrorDataResult<GetOrderItemDetailDto>(null, "Sipariş edilen ürün bulunamadı.");
        }

        var orderItemDetail = new GetOrderItemDetailDto
        {
            ProductName = orderItem.Product.ProductName,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            Description = orderItem.Product.Description,
            ImageUrl = orderItem.Product.ImageUrl,
        };
        return new SuccessDataResult<GetOrderItemDetailDto>(orderItemDetail, "Sipariş edilen ürün başarıyla getirildi.");       

    }

    public async Task<DataResult<decimal>> GetTotalOrderPriceAsync(int Id)
    {
        var order = await _orderRepository.GetByIdAsync(Id);
        if (order == null) 
        {
            return new ErrorDataResult<decimal>(0,"Böyle bir sipariş bulunamadı.");          
        }
        if (order.OrderItems == null || !order.OrderItems.Any())
        {
            return new SuccessDataResult<decimal>(0, "Siparişte ürün yok.");
        }
        decimal totalPrice = 0;

        foreach (var item in order.OrderItems) 
        {
            totalPrice += item.UnitPrice * item.Quantity;; 
        }

        return new SuccessDataResult<decimal>(totalPrice, "Siparişin toplam fiyatı getirildi.");
              
    }

    
}
