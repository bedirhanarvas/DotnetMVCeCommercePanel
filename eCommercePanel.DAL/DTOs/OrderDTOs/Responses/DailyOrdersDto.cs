using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.OrderDTOs.Responses;

public class DailyOrdersDto
{
    public DateTime Date { get; set; }
    public int TotalQuantity { get; set; }
}
