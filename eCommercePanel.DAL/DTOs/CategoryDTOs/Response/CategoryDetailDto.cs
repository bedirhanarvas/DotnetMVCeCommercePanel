using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.CategoryDTOs.Response;

public class CategoryDetailDto
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
