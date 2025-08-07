using eCommercePanel.BLL.Authentication.Abstracts;
using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.UserDTOs.Requets;
using eCommercePanel.DAL.DTOs.UserDTOs.Responses;
using eCommercePanel.DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eCommercePanel.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IReportService _reportService;
    private readonly IAuthService _authService;

    public UserController(IUserService userService, IReportService reportService, IAuthService authService)
    {
        _userService = userService;
        _reportService = reportService;
        _authService = authService;
    }

    [HttpGet]
    public async Task<IActionResult> UserList()
    {
        var result = await _userService.GetAllUsers();

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(new List<UserDto>());
        }

        return View(result.Data);
    }
 
    [HttpGet]
    public async Task<IActionResult> UserDetail()
    {
        var userIdClaim = _authService.GetClaimsUserInfoAsync();

        if (userIdClaim.Id == null)
        {
            TempData["Error"] = "Giriş yapan kullanıcı bilgisi alınamadı.";
            return RedirectToAction("Login"); // Gerekirse login sayfasına yönlendir
        }

        var result = await _userService.GetByIdAsync(userIdClaim.Id);

        //var result = await _userService.GetByIdAsync(id);

        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message ?? "Kullanıcı bulunamadı.";
            return RedirectToAction("UserList");
        }

        return View(result.Data);
    }

    
    [HttpGet]
    public IActionResult UserAdd()
    {
        return View(new UserCreateDto());
    }

    
    [HttpPost]
    public async Task<IActionResult> UserAdd(UserCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _userService.AddAsync(dto);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(dto);
        }

        return RedirectToAction("UserList");
    }

    
    [HttpGet]
    public async Task<IActionResult> UserDelete()
    {
        var userIdClaim = _authService.GetClaimsUserInfoAsync();

        if (userIdClaim.Id == null)
        {
            TempData["Error"] = "Giriş yapan kullanıcı bilgisi alınamadı.";
            return RedirectToAction("Login"); // Gerekirse login sayfasına yönlendir
        }
        await _userService.DeleteAsync(userIdClaim.Id);
        
        return RedirectToAction("Login");
    }

    
    [HttpGet]
    public async Task<IActionResult> UserEdit(int id)
    {
        var result = await _userService.GetByIdAsync(id);

        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message ?? "Kullanıcı bulunamadı.";
            return RedirectToAction("UserList");
        }

        var dto = new UserUpdateDto
        {
            id = id,
            FirstName = result.Data.FirstName,
            LastName = result.Data.LastName,
            Email = result.Data.Email,
            // Şifre gösterilmez (istersen ayrı alan açarsın)
        };

        return View(dto);
    }

    
    [HttpPost]
    public async Task<IActionResult> UserEdit(UserUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _userService.UpdateAsync(dto);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(dto);
        }

        return RedirectToAction("UserList");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserCreateDto userCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return View(userCreateDto);
        }

        var result = await _userService.AddAsync(userCreateDto);
        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(userCreateDto);
        }

        TempData["Success"] = "Kayıt başarılı, giriş yapabilirsiniz.";
        return RedirectToAction("Login");
    }

    [HttpGet("/account-login")]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost("/account-login")]
  
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
            return View(userLoginDto);

        var result = await _userService.LoginAsync(userLoginDto);
        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(userLoginDto);
        }

        var newClaim = new[]
               {
                    new Claim("UserId", result.Data.Id.ToString()),
                    new Claim("NameSurname", result.Data.FirstName + " " + result.Data.LastName),
                    new Claim("Email", result.Data.Email),
                };



        bool check = await _authService.AddClaimAsync(newClaim);

        TempData["Welcome"] = $"Hoş geldin, {result.Data.FirstName}!";

        return RedirectToAction("Index", "Dashboard");
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "User");
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(UserForgotPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var user = await _userService.GetUserByEmail(dto.Email); 
        if (user == null)
        {
            ViewBag.Error = "Bu e-posta ile kayıtlı kullanıcı bulunamadı.";
            return View(dto);
        }

        TempData["Message"] = "Şifre sıfırlama bağlantısı gönderildi (simülasyon).";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> GetOrdersByUserId(int userId)
    {
        var orders = await _reportService.GetOrdersByUserIdAsync(userId);
        return PartialView("UserOrdersPartial", orders);
    }
}
