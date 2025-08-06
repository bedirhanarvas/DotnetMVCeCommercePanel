using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.Context;
using eCommercePanel.DAL.DTOs.OrderDTOs.Responses;
using eCommercePanel.DAL.DTOs.ProductDTOs.Responses;
using eCommercePanel.DAL.DTOs.ReportDTOs.Requests;
using eCommercePanel.DAL.DTOs.UserDTOs.Responses;
using eCommercePanel.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace eCommercePanel.BLL.Managers;

public class ReportManager : IReportService
{
    private readonly AppDbContext _context;

    public ReportManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TopSellingProductDto> GetTopSellingProductAsync()
    {
        var result = await _context.OrderItems
            .Include(oi => oi.Product)
            .GroupBy(oi => oi.ProductId)
            .Select(g => new TopSellingProductDto
            {
                ProductName = g.First().Product.ProductName,
                TotalSold = g.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(g => g.TotalSold)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<Product> GetMostExpensiveProductAsync()
    {
        return await _context.Products
            .OrderByDescending(p => p.Price)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetSoldProductsCountTodayAsync()
    {
        var today = DateTime.Today; 

        var soldProductsCount = await _context.Orders
            .Where(o => o.OrderDate.Date == today)  
            .SelectMany(o => o.OrderItems)  
            .SumAsync(oi => oi.Quantity);  

        return soldProductsCount;
    }

    public async Task<List<PreparingOrderDto>> GetPreparingOrdersAsync()
    {
        return await _context.Orders
            .Where(o => o.Status == "Hazırlanıyor")
            .Include(o => o.User)
            .Include(o => o.Address)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Select(o => new PreparingOrderDto
            {
                Id = o.Id,
                CustomerFullName = o.User.FirstName + " " + o.User.LastName,
                AddressLine = o.Address.AddressLine,
                City = o.Address.City,
                Products = o.OrderItems
                .GroupBy(oi => oi.Product.ProductName)
                .Select(g => new ProductSummaryDto
                {
                    ProductName = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<List<DailyOrdersDto>> GetDailySalesAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var start = startDate ?? DateTime.Today.AddDays(-6);
        var end = endDate ?? DateTime.Today;

        var sales = await _context.Orders
            .Where(o => o.OrderDate.Date >= start.Date && o.OrderDate.Date <= end.Date)
            .SelectMany(o => o.OrderItems, (order, item) => new
            {
                order.OrderDate,
                item.Quantity
            })
            .GroupBy(x => x.OrderDate.Date)
            .Select(g => new DailyOrdersDto
            {
                Date = g.Key,
                TotalQuantity = g.Sum(x => x.Quantity)
            })
            .ToListAsync();

        // Tüm günleri oluştur (0 olanları da dahil)
        var fullList = Enumerable.Range(0, (end - start).Days + 1)
            .Select(offset =>
            {
                var date = start.AddDays(offset).Date;
                var matched = sales.FirstOrDefault(s => s.Date == date);
                return new DailyOrdersDto
                {
                    Date = date,
                    TotalQuantity = matched?.TotalQuantity ?? 0
                };
            })
            .ToList();

        return fullList;
    }

    public async Task<List<UserOrderDetailDto>> GetOrdersByUserIdAsync(int userId)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Address)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return orders.Select(order => new UserOrderDetailDto
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Address = $"{order.Address?.AddressLine}, {order.Address?.City}",
            Items = order.OrderItems.Select(oi => new OrderItemDto
            {
                ProductName = oi.Product.ProductName,
                Quantity = oi.Quantity
            }).ToList()
        }).ToList();
    }



}

