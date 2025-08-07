using eCommercePanel.BLL.Results;
using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.OrderDTOs.Responses;
using eCommercePanel.DAL.DTOs.UserDTOs.Requets;
using eCommercePanel.DAL.DTOs.UserDTOs.Responses;
using eCommercePanel.DAL.Entities;
using eCommercePanel.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace eCommercePanel.BLL.Managers;

public class UserManager : IUserService
{ 
    private readonly IUserRepository _userRepository;

    public UserManager(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> AddAsync(UserCreateDto userCreateDto)
    {
        var user = await _userRepository.GetByEmailAsync(userCreateDto.Email);
        
        if (user != null && user.Email == userCreateDto.Email) {

            return new ErrorResult("Bu kullanıcı zaten kayıtlı.");
        }

        var newUser = new User()
        {
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            Email = userCreateDto.Email,
            Password = Hash(userCreateDto.Password),
            CreatedAt = userCreateDto.CreatedAt
        };
        
        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveAsync();
             
        return new SuccessResult("Başarıyla kaydedildi.");
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return new ErrorResult("Böyle bir kullanıcı mevcut değil.");
        }
        _userRepository.Delete(user);
        await _userRepository.SaveAsync();
        return new SuccessResult("Kulllanıcı başarıyla silindi.");
    }

    public async Task<DataResult<List<UserDto>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();

        if (users == null)
        {
            return new ErrorDataResult<List<UserDto>>(null, "Henüz hiçbir kullanıcı bulunmuyor.");
        }

        var userDto = users.Select(p => new UserDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Email = p.Email,            
        }).ToList();

        return new SuccessDataResult<List<UserDto>>(userDto,"Kullanıcılar listelendi.");
    }

    public async Task<DataResult<UserDetailDto>> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if(user == null)
        {
            return new ErrorDataResult<UserDetailDto>(null, "");
        }
        var userDetailDto = new UserDetailDto
        {
            FirstName =user.FirstName,
            LastName =user.LastName,
            Email =user.Email,
        };
        return new SuccessDataResult<UserDetailDto>(userDetailDto,"");
    }

    public async Task<DataResult<UserDetailDto>> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if(user == null)
        {
            return new ErrorDataResult<UserDetailDto> (null,"Kullanıcı bulunamadı.");
        }

        var userDetail = new UserDetailDto()
        {
            FirstName=user.FirstName,
            LastName=user.LastName,
            Email =user.Email,
        };

        return new SuccessDataResult<UserDetailDto>(userDetail,"Kullanıcı bulundu.");
    }

    public async Task<Result> UpdateAsync(UserUpdateDto userUpdateDto)
    {
        var user = await _userRepository.GetByIdAsync(userUpdateDto.id);
        if(user == null)
        {
            return new ErrorResult("Kullanıcı bulunamadı.");
        }
        if (!string.IsNullOrEmpty(userUpdateDto.FirstName))
        {
            user.FirstName = userUpdateDto.FirstName;
            user.UpdatedAt = DateTime.Now;
        }
        if (!string.IsNullOrEmpty(userUpdateDto.LastName))
        {
            user.LastName = userUpdateDto.LastName;
            user.UpdatedAt = DateTime.Now;
        }
        if (!string.IsNullOrEmpty(userUpdateDto.Email))
        {
            user.Email = userUpdateDto.Email;
            user.UpdatedAt = DateTime.Now;
        }
        if(!string.IsNullOrEmpty(userUpdateDto.Password))
        {
            user.Password = userUpdateDto.Password;
            user.UpdatedAt = DateTime.Now;
        }

        await _userRepository.SaveAsync();
        return new SuccessResult("Kullanıcı başarıyla güncellendi.");
    }
    public async Task<DataResult<UserDto>> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);
        if (user == null)
        {
            return new ErrorDataResult<UserDto>(null, "Kullanıcı bulunamadı.");
        }

        
        var hashedPassword = Hash(userLoginDto.Password);
        if (user.Password != hashedPassword)
        {
            return new ErrorDataResult<UserDto>(null, "Şifre hatalı.");
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        return new SuccessDataResult<UserDto>(userDto, "Giriş başarılı.");
    }

    public static string Hash(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public async Task<List<UserOrderDetailDto>> GetOrdersByUserIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);


   

        return null;
    }

}
