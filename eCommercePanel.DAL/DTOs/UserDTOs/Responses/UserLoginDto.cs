using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.UserDTOs.Responses;

public class UserLoginDto
{
    [Required(ErrorMessage = "Email boş bırakılamaz.")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
