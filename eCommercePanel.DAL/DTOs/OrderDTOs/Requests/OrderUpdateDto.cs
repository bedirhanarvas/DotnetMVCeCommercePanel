using eCommercePanel.DAL.DTOs.OrderItemDTOs.Requests;
using System.ComponentModel.DataAnnotations;

namespace eCommercePanel.DAL.DTOs.OrderDTOs.Requests;

public class OrderUpdateDto
{
    [Required]
    public int Id { get; set; } 

    public int? AddressId { get; set; }  

    [StringLength(50)]
    public string? Status { get; set; }

    public List<OrderItemUpdateDto>? OrderItems { get; set; }

}
