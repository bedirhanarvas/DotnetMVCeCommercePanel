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
    
    [Required]
    public int UserId { get; set; }
    public virtual User User { get; set; } = new();

    [Required]
    public int AddressId { get; set; }
    public virtual Address? Address { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; }= new List<OrderItem>();
    

}
