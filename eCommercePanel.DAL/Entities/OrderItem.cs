using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommercePanel.DAL.Entities;

[Table("OrderItems")]
public class OrderItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }



    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }


    public int OrderId { get; set; }
    public Order? Order { get; set; }


    public int ProductId { get; set; }
    public Product Product { get; set; }
}
