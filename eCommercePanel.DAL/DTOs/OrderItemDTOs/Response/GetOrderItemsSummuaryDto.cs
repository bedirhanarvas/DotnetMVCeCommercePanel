using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.OrderItemDTOs.Response;

public class GetOrderItemsSummuaryDto
{
    public int OrderItemId { get; set; }
    public string ProductName { get; set; }
    public decimal TotalPrice { get; set; }
}
