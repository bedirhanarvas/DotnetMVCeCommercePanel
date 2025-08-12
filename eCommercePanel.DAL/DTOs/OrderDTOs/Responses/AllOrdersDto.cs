using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.OrderDTOs.Responses;

public class AllOrdersDto
{
    public int OrderId { get; set; }       // Sipariş ID
    public int UserId { get; set; }        // Kullanıcı ID
    public string UserFullName { get; set; } // Kullanıcının adı soyadı
    public int AddressId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Hazırlanıyor";

    public List<OrderItemDto> OrderItems { get; set; }
}
