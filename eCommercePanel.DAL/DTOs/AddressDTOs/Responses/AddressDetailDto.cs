using eCommercePanel.DAL.DTOs.UserDTOs.Responses;

namespace eCommercePanel.DAL.DTOs.AddressDTOs.Responses;

public class AddressDetailDto
{

    public UserDto UserDto { get; set; }
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
}
