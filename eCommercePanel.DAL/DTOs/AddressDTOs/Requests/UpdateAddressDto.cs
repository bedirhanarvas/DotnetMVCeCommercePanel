namespace eCommercePanel.DAL.DTOs.AddressDTOs.Requests;

public class UpdateAddressDto
{
    public int id {  get; set; }
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
}
