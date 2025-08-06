using eCommercePanel.DAL.DTOs.OrderDTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.UserDTOs.Responses;

public class UserOrderDetailDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Address { get; set; }
    public List<OrderItemDto> Items { get; set; }
}
