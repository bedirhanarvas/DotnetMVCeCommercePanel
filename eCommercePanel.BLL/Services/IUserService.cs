using eCommercePanel.BLL.Results;
using eCommercePanel.DAL.DTOs.UserDTOs.Requets;
using eCommercePanel.DAL.DTOs.UserDTOs.Responses;
using eCommercePanel.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.BLL.Services;

public interface IUserService
{
    Task<DataResult<List<UserDto>>> GetAllUsers();
    Task<DataResult<UserDetailDto>> GetUserByEmail(string email);
    Task<DataResult<UserDetailDto>> GetByIdAsync(int id);
    Task<DataResult<UserDto>> LoginAsync(UserLoginDto dto);
    Task<Result> AddAsync(UserCreateDto userCreateDto);
    Task<Result> UpdateAsync(UserUpdateDto userUpdateDto);
    Task<Result> DeleteAsync(int id);

}
