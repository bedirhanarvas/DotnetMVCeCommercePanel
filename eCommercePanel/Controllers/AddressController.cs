using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.AddressDTOs.Requests;
using eCommercePanel.DAL.DTOs.AddressDTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace eCommercePanel.Controllers;

public class AddressController:Controller
{
    private readonly IAddressService _addressService;
    private readonly IUserService _userService;

    public AddressController(IAddressService addressService, IUserService userService)
    {
        _addressService = addressService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult>  AddressDetail(int id)
    {
        var result = await _addressService.GetByIdAsync(id);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return RedirectToAction("AddressList");
        }
        return View(result.Data);
    }

    [HttpGet]
    public IActionResult AddressAdd()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddressAdd(CreateAddressDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _addressService.AddAsync(dto);
        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(dto);
        }

        return RedirectToAction("AddressList");
    }
    [HttpGet]
    public async Task<IActionResult> AddressEdit(int id)
    {
        var result = await _addressService.GetByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("AddressList");
        }

        var updateDto = new UpdateAddressDto
        {
            id = id,
            AddressLine = result.Data.AddressLine,
            City = result.Data.City,
            PostalCode = result.Data.PostalCode,
            Country = result.Data.Country
        };

        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddressEdit(UpdateAddressDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _addressService.UpdateAsync(dto);
        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(dto);
        }

        return RedirectToAction("AddressDetail", new { id = dto.id });
    }

    [HttpPost]
    public async Task<IActionResult> AddressDelete(int id)
    {
        var result = await _addressService.DeleteAsync(id);
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
        }

        return RedirectToAction("AddressList");
    }

    [HttpGet]
    public async Task<IActionResult> AddressList(int userId)
    {
        var result = await _addressService.GetAddressesByUserIdAsync(userId);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(new List<AddressDetailDto>());
        }

        return View(result.Data);
    }
}
