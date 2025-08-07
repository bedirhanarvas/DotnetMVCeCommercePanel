using eCommercePanel.BLL.Authentication.Abstracts;
using eCommercePanel.DAL.DTOs.UserDTOs.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace eCommercePanel.BLL.Authentication.Concretes;

    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AddClaimAsync(Claim[] claims)
        {
            try
            {
                var userPrincipal = _httpContextAccessor.HttpContext.User;
                var currentClaims = userPrincipal.Claims.ToList();
                foreach (var claim in claims)
                {
                    currentClaims.Add(claim);
                }
                var newIdentity = new ClaimsIdentity(currentClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var newPrincipal = new ClaimsPrincipal(newIdentity);
                await _httpContextAccessor.HttpContext.SignOutAsync();
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newPrincipal);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task ClearClaimsAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }

        public async Task<Claim[]> GetClaimsAsync()
        {
            var userPrincipal = _httpContextAccessor.HttpContext.User;
            var userClaims = await Task.FromResult(userPrincipal.Claims.ToArray());
            return userClaims;
        }

        public async Task<string[]> GetRolesAsync()
        {
            var userPrincipal = _httpContextAccessor.HttpContext.User;
            var roleClaims = userPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            return await Task.FromResult(roleClaims);
        }

        public async Task<bool> IsUserInRoleAsync(string roleName)
        {
            var userPrincipal = _httpContextAccessor.HttpContext.User;
            return await Task.FromResult(userPrincipal.IsInRole(roleName));
        }

        public async Task<bool> RemoveClaimAsync(Claim claim)
        {
            try
            {
                var userPrincipal = _httpContextAccessor.HttpContext.User;
                var currentClaims = userPrincipal.Claims.ToList();
                var claimToRemove = currentClaims.FirstOrDefault(c => c.Type == claim.Type && c.Value == claim.Value);

                if (claimToRemove != null)
                {
                    currentClaims.Remove(claimToRemove);
                    var newIdentity = new ClaimsIdentity(currentClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var newPrincipal = new ClaimsPrincipal(newIdentity);

                    await _httpContextAccessor.HttpContext.SignOutAsync();
                    await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newPrincipal);

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SetClaimsAndSignAsync(Claim[] claims)
        {
            try
            {
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateClaimsAsync(UserDto user)
        {
            try
            {
                await _httpContextAccessor.HttpContext.SignOutAsync();

                var claims = new List<Claim>
                     {
                         new Claim("UserId", user.Id.ToString()),
                         new Claim("Name", user.FirstName),
                         new Claim("LastName", user.LastName),
                         new Claim("Email", user.Email),
                     };

                var newIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var newPrincipal = new ClaimsPrincipal(newIdentity);
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newPrincipal);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }

        private async Task SignInWithClaimsAsync(Claim[] claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }


        private TModel ConvertClaimsToModel<TModel>(List<Claim> claims)
        {
            // Bu metot içinde belirli bir DTO/model dönüşümünü gerçekleştirebilirsiniz.
            // Örneğin:
            if (typeof(TModel) == typeof(UserDto))
            {
                //var userDto = new UserDTO();
                //userDto.userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name )?.Value;
                //userDto.userSurname = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                //userDto.userID = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                //userDto.userEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                // Diğer özellikleri dönüştürerek ekleyebilirsiniz...
                //return (TModel)(object)userDto;
            }

            // Diğer DTO/model türleri için dönüşüm işlemlerini ekleyebilirsiniz...

            return default(TModel);
        }

        public async Task<Claim> GetClaimsAsync(string claimType)
        {
            var userPrincipal = _httpContextAccessor.HttpContext.User;

            var userClaims = await Task.FromResult(userPrincipal.Claims.ToArray());

            var filteredClaims = userClaims.FirstOrDefault(c => c.Type == claimType);

            return filteredClaims;
        }

        public UserDto GetClaimsUserInfoAsync()
        {
            UserDto user = new UserDto();
            user.Id = Convert.ToInt32(_httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value);
            user.FirstName = _httpContextAccessor.HttpContext.User?.FindFirst("Name")?.Value;
            user.LastName = _httpContextAccessor.HttpContext.User?.FindFirst("LastName")?.Value;
            user.Email = _httpContextAccessor.HttpContext.User?.FindFirst("Email")?.Value;

            return user;
        }
    }

