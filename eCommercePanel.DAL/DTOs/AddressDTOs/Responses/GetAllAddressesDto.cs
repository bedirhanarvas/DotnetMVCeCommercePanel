namespace eCommercePanel.DAL.DTOs.AddressDTOs.Responses;

public class GetAllAddressesDto
{
    public int? UserId { get; set; }
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

}
