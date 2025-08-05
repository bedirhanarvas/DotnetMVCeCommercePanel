using eCommercePanel.DAL.DTOs.ProductDTOs.Responses;
using eCommercePanel.DAL.DTOs.ReportDTOs.Requests;
using eCommercePanel.DAL.Entities;

namespace eCommercePanel.Models;

public class DashboardViewModel
{
    public TopSellingProductDto TopSellingProduct { get; set; }
    public Product MostExpensiveProduct { get; set; }
    public List<ProductDetailDto> AllProducts { get; set; }



}