using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.Entities;

[Table("Addresses")]
    public class Address
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [Required]
    [StringLength(250)]
    public string? AddressLine {  get; set; }

    [Required]
    [StringLength(100)]
    public string? City { get; set; }

    [Required]
    [StringLength(20)]
    public string? PostalCode { get; set; }

    [Required]
    [StringLength(100)]
    public string? Country { get; set; }


    [Required]
    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
