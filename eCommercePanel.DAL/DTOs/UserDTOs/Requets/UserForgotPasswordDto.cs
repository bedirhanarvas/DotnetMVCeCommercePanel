using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.UserDTOs.Requets;

public class UserForgotPasswordDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi girin.")]
    public string Email { get; set; }
}
