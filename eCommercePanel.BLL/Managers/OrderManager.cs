using eCommercePanel.BLL.Results;
using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.AddressDTOs.Requests;
using eCommercePanel.DAL.DTOs.OrderDTOs.Requests;
using eCommercePanel.DAL.DTOs.OrderDTOs.Responses;
using eCommercePanel.DAL.Entities;
using eCommercePanel.DAL.Interfaces;

namespace eCommercePanel.BLL.Managers;

public class OrderManager : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IAddressRepository _addressRepository;
    public OrderManager(IOrderRepository orderRepository, IAddressRepository addressRepository)
    {
        _orderRepository = orderRepository;
        _addressRepository = addressRepository;
    }

    public async Task<Result> AddAsync(OrderCreateDto orderCreateDto)
    {
        if (orderCreateDto.OrderItems == null || !orderCreateDto.OrderItems.Any())
        {
            return new ErrorResult("Sipariş kalemi boş olamaz.");
        }


        var order = new Order()
        {
            AddressId = orderCreateDto.AddressId,
            OrderDate = DateTime.Now,
            UserId = orderCreateDto.UserId,
            Status = orderCreateDto.Status,
            OrderItems = orderCreateDto.OrderItems.Select(oi => new OrderItem
            {
                ProductId = oi.ProductId,
                UnitPrice = oi.UnitPrice,
                Quantity = oi.Quantity,
            }).ToList()
        };

        order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
        await _orderRepository.AddAsync(order);
        return new SuccessResult("Sipariş başarıyla oluşturuldu.");
    }

    public async Task<Result> DeleteOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            return new ErrorResult("Sipariş bulunamadı.");
        }

        _orderRepository.Delete(order);
        return new SuccessResult("Sipariş başarıyla silindi.");
    }

    public async Task<DataResult<List<AllOrdersDto>>> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllAsync();

        var orderDto = orders.Select(o => new AllOrdersDto
        {
            OrderId = o.Id,
            UserId = o.UserId,
            UserFullName = o.User != null ? o.User.FirstName + " " + o.User.LastName : "Bilinmiyor", // Null kontrolü
            AddressId = o.AddressId,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Status = o.Status,
            OrderItems = o.OrderItems != null ? o.OrderItems.Select(oi => new OrderItemDto
            {
                ProductName = oi.Product?.ProductName ?? "Ürün Bilgisi Yok", // Null kontrolü
                Quantity = oi.Quantity
            }).ToList() : new List<OrderItemDto>() // Null kontrolü
        }).ToList();

        return new SuccessDataResult<List<AllOrdersDto>>(orderDto);

    }

    public async Task<DataResult<OrderDetailDto>> GetOrderDetail(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
        {
            return new ErrorDataResult<OrderDetailDto>(null, "Sipariş bulunamadı.");
        }

        var orderDetail = new OrderDetailDto
        {
            UserId = order.UserId,
            AddressId = order.AddressId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
        };

        return new SuccessDataResult<OrderDetailDto>(orderDetail, "Ürün detayı başarıyla getirildi.");
    }

    public async Task<DataResult<List<OrderDetailDto>>> GetOrdersByUserIdAsync(int userId)
    {
        var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);



        var orderDetail = orders.Select(order => new OrderDetailDto
        {
            UserId = order.UserId,
            AddressId = order.AddressId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status,

        }).ToList();

        return new SuccessDataResult<List<OrderDetailDto>>(orderDetail, "Siparişler getirildi.");
    }

    public async Task<Result> UpdateAsync(OrderUpdateDto orderUpdateDto)
    {
        var order = await _orderRepository.GetByIdAsync(orderUpdateDto.Id);

        if (order == null)
        {
            return new ErrorResult("Böyle bir sipariş bulunmamaktadır.");
        }

        if (orderUpdateDto.AddressId.HasValue)
        {
            var address = await _addressRepository.GetByIdAsync(orderUpdateDto.AddressId.Value);

            if (address == null)
            {
                return new ErrorResult("Adres bulunamadı.");
            }
            order.AddressId = orderUpdateDto.AddressId.Value;
        }
        if (!string.IsNullOrEmpty(orderUpdateDto.Status))
        {
            order.Status = orderUpdateDto.Status;
        }

        if (orderUpdateDto.OrderItems != null && orderUpdateDto.OrderItems.Any())
        {
            order.OrderItems.Clear();

            foreach (var item in orderUpdateDto.OrderItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                });
            }

            order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
        }

        _orderRepository.Update(order);
        return new SuccessResult("Sipariş başarıyla güncellendi.");
    }

}
