using eCommercePanel.DAL.DTOs.UserDTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.BLL.Authentication.Abstracts;

public interface IAuthService
{
    UserDto GetClaimsUserInfoAsync();

    Task<bool> SetClaimsAndSignAsync(Claim[] claims);

    Task<Claim[]> GetClaimsAsync();

    Task<Claim> GetClaimsAsync(string claimType);

    Task ClearClaimsAsync();

    Task<bool> AddClaimAsync(Claim[] claims);

    Task<bool> RemoveClaimAsync(Claim claim);

    Task<bool> UpdateClaimsAsync(UserDto user);

    Task<bool> IsUserInRoleAsync(string roleName);

    Task<string[]> GetRolesAsync();

}
