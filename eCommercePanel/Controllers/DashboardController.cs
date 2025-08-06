using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.ProductDTOs.Responses;
using eCommercePanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommercePanel.Controllers;

public class DashboardController : Controller
{
    private readonly IReportService _reportService;
    private readonly IProductService _productService;

    public DashboardController(IReportService reportService, IProductService productService)
    {
        _reportService = reportService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var topProduct = await _reportService.GetTopSellingProductAsync();
        var mostExpensiveProduct = await _reportService.GetMostExpensiveProductAsync();
        var allProductsResult = await _productService.GetAllAsync();
        var allProducts = allProductsResult.Data;
        var totalStock = allProducts.Sum(p => p.Stock ?? 0);
        var preparingOrders = await _reportService.GetPreparingOrdersAsync();
        var dailySales = await _reportService.GetDailySalesAsync(DateTime.Today.AddDays(-7), DateTime.Today);


        var productDtos = allProducts.Select(p => new ProductDetailDto
        {
            ProductName = p.ProductName,
            Stock = p.Stock ?? 0,  
            StockPercentage = totalStock > 0 ? (p.Stock ?? 0) * 100 / totalStock : 0 
        }).ToList();

        var soldProductsToday = await _reportService.GetSoldProductsCountTodayAsync();

        var viewModel = new DashboardViewModel
        {
            TopSellingProduct = topProduct,
            MostExpensiveProduct = mostExpensiveProduct,
            AllProducts = productDtos,
            SoldProductsToday = soldProductsToday,
            PreparingOrdersCount = preparingOrders.Count,
            DailySales = dailySales
        };

        return View(viewModel);
    }

    public async Task<IActionResult> PreparingOrders()
    {
        var orders = await _reportService.GetPreparingOrdersAsync();
        return View(orders);
    }

   

}
