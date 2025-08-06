using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommercePanel.DAL.Entities;

[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "İsim boş bırakılamaz.")]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Soyad boş bırakılamaz.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email boş bırakılamaz.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    public string Email { get; set; }

    [StringLength(50, ErrorMessage = "Şifre en fazla 100 karakter olabilir.")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
