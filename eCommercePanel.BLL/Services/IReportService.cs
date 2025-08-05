using eCommercePanel.DAL.DTOs.ReportDTOs.Requests;
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
}
