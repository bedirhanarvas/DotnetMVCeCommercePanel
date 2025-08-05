using eCommercePanel.DAL.DTOs.OrderItemDTOs.Requests;
using System.ComponentModel.DataAnnotations;

namespace eCommercePanel.DAL.DTOs.AddressDTOs.Requests;

public class OrderCreateDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public int AddressId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Hazırlanıyor";

    public List<OrderItemCreateDto> OrderItems { get; set; } 
}
