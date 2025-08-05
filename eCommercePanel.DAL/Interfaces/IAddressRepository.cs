using eCommercePanel.DAL.DTOs.AddressDTOs.Requests;
using eCommercePanel.DAL.DTOs.AddressDTOs.Responses;
using eCommercePanel.DAL.Entities;

namespace eCommercePanel.DAL.Interfaces;

public interface IAddressRepository
{
    Task<Address> GetByIdAsync(int id);
    Task<List<Address>> GetUserAddressAsync(int userId);
    Task<List<Address>> GetMyAddressesAsync(int Id);
    Task<Address> GetByIdWithUserAsync(int id);
    Task<List<Address>> GetByUserIdAsync(int userId);

    Task AddAsync(Address address);
    void Delete(Address address);
    void UpdateAddress(Address address);
    Task SaveAsync();    

}
