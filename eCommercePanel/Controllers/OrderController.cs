using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.AddressDTOs.Requests;
using eCommercePanel.DAL.DTOs.OrderDTOs.Requests;
using eCommercePanel.DAL.DTOs.OrderDTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace eCommercePanel.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // Tüm siparişleri listele
    [HttpGet]
    public async Task<IActionResult> OrderList()
    {
        var result = await _orderService.GetAllOrders();

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(new List<AllOrdersDto>());
        }

        return View(result.Data);
    }

    // Sipariş detayı
    [HttpGet]
    public async Task<IActionResult> OrderDetail(int id)
    {
        var result = await _orderService.GetOrderDetail(id);

        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message ?? "Sipariş bulunamadı.";
            return RedirectToAction("OrderList");
        }

        return View(result.Data);
    }

    // Yeni sipariş formu (GET)
    [HttpGet]
    public IActionResult OrderAdd()
    {
        return View(new OrderCreateDto());
    }

    // Yeni sipariş oluşturma (POST)
    [HttpPost]
    public async Task<IActionResult> OrderAdd(OrderCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _orderService.AddAsync(dto);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(dto);
        }

        return RedirectToAction("OrderList");
    }

    // Sipariş silme
    [HttpGet]
    public async Task<IActionResult> OrderDelete(int id)
    {
        var result = await _orderService.DeleteOrderAsync(id);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
        }

        return RedirectToAction("OrderList");
    }

    // Sipariş güncelleme formu (GET)
    [HttpGet]
    public async Task<IActionResult> OrderEdit(int id)
    {
        var result = await _orderService.GetOrderDetail(id);

        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message ?? "Sipariş bulunamadı.";
            return RedirectToAction("OrderList");
        }

        var dto = new OrderUpdateDto
        {
            Id = id,
            AddressId = result.Data.AddressId,
            Status = result.Data.Status
            // OrderItems burada null bırakıldı, istersen elle ekleme yapabilirsin
        };

        return View(dto);
    }

    // Sipariş güncelleme işlemi (POST)
    [HttpPost]
    public async Task<IActionResult> OrderEdit(OrderUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _orderService.UpdateAsync(dto);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(dto);
        }

        return RedirectToAction("OrderList");
    }

    // Belirli bir kullanıcıya ait siparişleri listele
    [HttpGet]
    public async Task<IActionResult> OrdersByUser(int userId)
    {
        var result = await _orderService.GetOrdersByUserIdAsync(userId);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("OrderList");
        }

        return View(result.Data); // View adını "OrdersByUser.cshtml" olarak oluştur
    }
}

