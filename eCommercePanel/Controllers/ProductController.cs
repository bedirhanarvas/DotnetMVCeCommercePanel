using eCommercePanel.BLL.Results;
using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.ProductDTOs.Requests;
using eCommercePanel.DAL.DTOs.ProductDTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommercePanel.Controllers;

public class ProductController: Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;


    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ProductList()
    {
        var result = await _productService.GetAllAsync();

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(new List<GetAllProductsDto>());
        }
        return View(result.Data);
    }

    [HttpGet]

    public async Task<IActionResult> ProductDetail(int id)
    {
        var result = await _productService.GetByIdAsync(id);

        if (!result.Success || result.Data == null) { 
        
            ViewBag.Error = result.Message ?? "Ürün bulunamadı.";

            return RedirectToAction("ProductList");
        }

        return View(result.Data);
    }

    [HttpGet]
        public async Task<IActionResult> ProductAdd()
        {
        var categoriesResult = await _categoryService.GetAllAsync();

        if (!categoriesResult.Success)
        {
            ViewBag.Categories = new List<SelectListItem>();
            ViewBag.Error = categoriesResult.Message;
            return View();
        }

        // Listeyi DropDown için uygun hale getir
        ViewBag.Categories = new SelectList(categoriesResult.Data,"Id", "CategoryName");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProductAdd(ProductCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var categoriesResult = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categoriesResult.Data, "Id", "CategoryName");
            return View(dto);
        }

        //Console.WriteLine("categoryId= ",dto.CategoryId );

        var result = await _productService.AddAsync(dto);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            var categoriesResult = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categoriesResult.Data, "Id", "CategoryName");
            return View(dto);
        }

        return RedirectToAction("ProductList");
    }

    [HttpPost("/Product/ProductDelete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductDelete(int id)
    {
        var result = await _productService.DeleteAsync(id);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
        }

        return RedirectToAction("ProductList");
    }

    [HttpGet]
    public async Task<IActionResult> ProductEdit(int id)
    {
        var result = await _productService.GetByIdAsync(id);

        if (!result.Success || result.Data == null)
        {
            return NotFound();
        }

        // Mapping: ProductDetailDto → ProductUpdateDto (manuel yapılabilir)
        var updateDto = new ProductUpdateDto
        {
            Id = id,
            ProductName = result.Data.ProductName,
            Price = result.Data.Price,
            Description = result.Data.Description,
        };

        return PartialView("ProductEdit", updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> ProductEdit(ProductUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _productService.UpdateAsync(dto);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(dto);
        }

        return RedirectToAction("ProductList");
    }

}
