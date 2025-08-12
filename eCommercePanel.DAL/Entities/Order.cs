using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommercePanel.DAL.Entities;

[Table("Orders")]
public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Hazırlanıyor";

    [ForeignKey("UserId")]
    public int UserId { get; set; }
    public User User { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }= new List<OrderItem>();
    

}
