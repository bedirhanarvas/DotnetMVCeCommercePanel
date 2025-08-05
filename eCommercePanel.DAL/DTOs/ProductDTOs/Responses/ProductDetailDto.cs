using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.ProductDTOs.Responses;

public class ProductDetailDto
{
    public string? ProductName { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public int StockPercentage { get; set; }
    public string ImageUrl { get; set; }
}
