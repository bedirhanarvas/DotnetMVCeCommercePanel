using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.ReportDTOs.Requests;

public class TopSellingProductDto
{
    public string ProductName { get; set; }
    public int TotalSold { get; set; }
}
