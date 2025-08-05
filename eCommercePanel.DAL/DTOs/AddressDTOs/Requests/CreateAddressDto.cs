﻿namespace eCommercePanel.DAL.DTOs.AddressDTOs.Requests;

public class CreateAddressDto
{
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
}
