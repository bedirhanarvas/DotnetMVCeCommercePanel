using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.OrderItemDTOs.Response;

public class GetOrderItemDetailDto
{
    public int OrderItemId { get; set; } 
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}
