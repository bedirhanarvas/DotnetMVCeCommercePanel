using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.ProductDTOs.Requests;

public class ProductUpdateDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public string? ImageUrl { get; set; }
}
