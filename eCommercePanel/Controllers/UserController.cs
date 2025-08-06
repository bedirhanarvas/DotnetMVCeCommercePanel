using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.UserDTOs.Requets;
using eCommercePanel.DAL.DTOs.UserDTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace eCommercePanel.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IReportService _reportService;

    public UserController(IUserService userService, IReportService reportService)
    {
        _userService = userService;
        _reportService = reportService;
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
    /// <summary>
    /// bu endpoint ve fonksiyon kullancıı detayşatıı getirir
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> UserDetail(int id)
    {
        var result = await _userService.GetByIdAsync(id);

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
    public async Task<IActionResult> UserDelete(int id)
    {
        var result = await _userService.DeleteAsync(id);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("Login");
        }
        
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

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
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

        //HttpContext.Session.SetString("Email", result.Data.Email);
        TempData["Welcome"] = $"Hoş geldin, {result.Data.FirstName}!";

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public IActionResult Logout()
    {
       
        TempData["Logout"] = "Başarıyla çıkış yaptınız.";
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
