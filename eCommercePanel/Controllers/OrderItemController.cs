using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.OrderItemDTOs.Requests;
using eCommercePanel.DAL.DTOs.OrderItemDTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


public class OrderItemController : Controller
{
    private readonly IOrderItemService _orderItemService;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;

    public OrderItemController(IOrderItemService orderItemService, IProductService productService, IOrderService orderService)
    {
        _orderItemService = orderItemService;
        _productService = productService;
        _orderService = orderService;

    }

    [HttpGet]
    public async Task<IActionResult> OrderItemList()
    {
        var result = await _orderItemService.GetAllAsync();
        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(new List<GetAllOrderItemsDto>());
        }

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> OrderItemsByUser(int userId)
    {
        var result = await _orderItemService.GetByUserIdAsync(userId);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View("OrderItemList", new List<GetAllOrderItemsDto>());
        }

        return View("OrderItemList",result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> OrderItemAdd()
    {
        var products = await _productService.GetAllAsync();
        var orders = await _orderService.GetAllOrders();

        ViewBag.Products = new SelectList(products.Data, "Id", "ProductName");
        ViewBag.Orders = new SelectList(orders.Data, "Id", "Id");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> OrderItemAdd(OrderItemCreateDto orderItemCreateDto)
    {
        if (!ModelState.IsValid)
        {
            var products = await _productService.GetAllAsync();
            var orders = await _orderService.GetAllOrders();

            ViewBag.Products = new SelectList(products.Data, "Id", "ProductName");
            ViewBag.Orders = new SelectList(orders.Data, "Id", "Id");
            return View(orderItemCreateDto);
        }

        var result = await _orderItemService.AddAsync(orderItemCreateDto);
        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(orderItemCreateDto);
        }

        return RedirectToAction("OrderItemList");
    }

    [HttpPost]
    public async Task<IActionResult> OrderItemDelete(int id)
    {
        var result = await _orderItemService.GetByIdAsync(id);

        if (!result.Success)
        {

            TempData["Error"] = result.Message;
        }

        return RedirectToAction("OrderItemList");
    }

    [HttpGet]
    public async Task<IActionResult> OrderItemDetail(int id)
    {
        var result = await _orderItemService.GetByIdAsync(id);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return RedirectToAction("OrderItemList");
        }
        return View(result.Data);
    }

}
