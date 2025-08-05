using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.Context;
using eCommercePanel.DAL.DTOs.ReportDTOs.Requests;
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
}

