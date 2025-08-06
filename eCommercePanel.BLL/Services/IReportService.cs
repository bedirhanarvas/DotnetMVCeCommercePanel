using eCommercePanel.DAL.DTOs.OrderDTOs.Responses;
using eCommercePanel.DAL.DTOs.ReportDTOs.Requests;
using eCommercePanel.DAL.DTOs.UserDTOs.Responses;
using eCommercePanel.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.BLL.Services;

public interface IReportService
{
    Task<TopSellingProductDto> GetTopSellingProductAsync();
    Task<Product> GetMostExpensiveProductAsync();
    Task<int> GetSoldProductsCountTodayAsync();
    Task<List<PreparingOrderDto>> GetPreparingOrdersAsync();
    Task<List<DailyOrdersDto>> GetDailySalesAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<List<UserOrderDetailDto>> GetOrdersByUserIdAsync(int userId);


}
