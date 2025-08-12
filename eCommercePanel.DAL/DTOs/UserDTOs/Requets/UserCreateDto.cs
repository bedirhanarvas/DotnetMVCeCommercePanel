using eCommercePanel.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.UserDTOs.Requets;

public class UserCreateDto
{
    public int id { get; set; }
    public string FirstName {  get; set; }
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email boş olamaz.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email giriniz.")]
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}
