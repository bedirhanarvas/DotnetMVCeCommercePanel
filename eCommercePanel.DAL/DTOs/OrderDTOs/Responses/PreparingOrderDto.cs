using eCommercePanel.DAL.DTOs.ProductDTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.DTOs.OrderDTOs.Responses;

public class PreparingOrderDto
{
    public int Id { get; set; }
    public string CustomerFullName { get; set; }
    public string AddressLine { get; set; }
    public string City { get; set; }
    public List<ProductSummaryDto> Products { get; set; }
}
