using eCommercePanel.BLL.Results;
using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.AddressDTOs.Requests;
using eCommercePanel.DAL.DTOs.AddressDTOs.Responses;
using eCommercePanel.DAL.DTOs.UserDTOs.Responses;
using eCommercePanel.DAL.Entities;
using eCommercePanel.DAL.Interfaces;

namespace eCommercePanel.BLL.Managers;

public class AddressManager : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;

    public AddressManager(IAddressRepository addressRepository, IUserRepository userRepository)
    {
        _addressRepository = addressRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> AddAsync(CreateAddressDto createAddressDto)
    {
        var address = new Address
        {
            AddressLine = createAddressDto.AddressLine,
            City = createAddressDto.City,
            PostalCode = createAddressDto.PostalCode,
            Country = createAddressDto.Country,
        };

        await _addressRepository.AddAsync(address);
        await _addressRepository.SaveAsync();
        return new SuccessResult();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null)
        {

            return new ErrorResult("Adres bulunamadı.");
        }

        _addressRepository.Delete(address);

        return new SuccessResult("Adresini başarıyla silindi.");
    }

    public async Task<DataResult<AddressDetailDto>> GetByIdAsync(int id)
    {
        var address = await _addressRepository.GetByIdWithUserAsync(id);
        if (address == null)
        {
            return new ErrorDataResult<AddressDetailDto>(null, "Adresiniz bulunamadı.");
        }
        var addressDetail = new AddressDetailDto
        {
            AddressLine = address.AddressLine,
            City = address.City,
            PostalCode = address.PostalCode,
            Country = address.Country,
            UserDto = new UserDto
            {
                FirstName = address.User.FirstName,
                LastName = address.User.LastName,
                Email = address.User.Email
            }
        };
        return new SuccessDataResult<AddressDetailDto>(addressDetail, "Adresinize ulaştınız.");
    }

    public async Task<Result> UpdateAsync(UpdateAddressDto updateAddressDto)
    {
        var address = await _addressRepository.GetByIdAsync(updateAddressDto.id);
        if (address == null) {

            return new ErrorResult("Adres bulunamadı.");
        }
        if (!string.IsNullOrEmpty(updateAddressDto.AddressLine))
        {
            address.AddressLine = updateAddressDto.AddressLine;
        }
        if (!string.IsNullOrEmpty(updateAddressDto.City))
        {
            address.City = updateAddressDto.City;
        }
        if (!string.IsNullOrEmpty(updateAddressDto.Country))
        {
            address.Country = updateAddressDto.Country;
        }
        if (!string.IsNullOrEmpty(updateAddressDto.PostalCode))
        {
            address.PostalCode = updateAddressDto.PostalCode;
        }
        await _addressRepository.SaveAsync();

        return new SuccessResult("Adresiniz başarıyla güncellendi.");
    }
    public async Task<DataResult<List<AddressDetailDto>>> GetAddressesByUserIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return new ErrorDataResult<List<AddressDetailDto>>(null, "Kullanıcı bulunamadı.");

        var addresses = await _addressRepository.GetMyAddressesAsync(userId);
        if (addresses == null || !addresses.Any())
            return new ErrorDataResult<List<AddressDetailDto>>(null, "Kullanıcının adresi bulunamadı.");

        var result = addresses.Select(address => new AddressDetailDto
        {
            AddressLine = address.AddressLine,
            City = address.City,
            PostalCode = address.PostalCode,
            Country = address.Country,
            UserDto = new UserDto
            {
                FirstName = address.User.FirstName,
                LastName = address.User.LastName,
                Email = address.User.Email
            }
        }).ToList();

        return new SuccessDataResult<List<AddressDetailDto>>(result, "Adresler başarıyla listelendi.");
    }
}
