using eCommercePanel.BLL.Results;
using eCommercePanel.DAL.DTOs.AddressDTOs.Requests;
using eCommercePanel.DAL.DTOs.AddressDTOs.Responses;
using eCommercePanel.DAL.Entities;

namespace eCommercePanel.BLL.Services;

public interface IAddressService
{

    Task<DataResult<AddressDetailDto>> GetByIdAsync(int id);
    Task<Result> AddAsync(CreateAddressDto createAddressDto);
    Task<Result> UpdateAsync(UpdateAddressDto updateAddressDto);
    Task<Result> DeleteAsync(int id);
    Task<DataResult<List<AddressDetailDto>>> GetAddressesByUserIdAsync(int userId);

}
