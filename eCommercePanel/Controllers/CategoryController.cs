using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.CategoryDTOs.Requests;
using eCommercePanel.DAL.DTOs.CategoryDTOs.Response;
using Microsoft.AspNetCore.Mvc;


public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> CategoryList()
    {
        var result = await _categoryService.GetAllAsync();

        if (!result.Success)
        {

            ViewBag.Error = result.Message;
            return View(new List<GetAllCategoriesDto>());
        }
        return View(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> CategoryDetail(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);

        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message ?? "Kategori bulunamadı.";
            return RedirectToAction("CategoryList");
        }

        return View(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> CategoryAdd()
    {
        return View(new CategoryCreateDto());
    }

    [HttpPost]
    public async Task<IActionResult> CategoryAdd(CategoryCreateDto categoryCreateDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Kategori ismi ve açıklaması boş olamaz.");
            return View(categoryCreateDto);
        }

        var result = await _categoryService.AddAsync(categoryCreateDto);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View(categoryCreateDto);
        }

        return RedirectToAction("CategoryList");
    }


    [HttpGet]
    public async Task<IActionResult> CategoryDelete(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);

        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message ?? "Kategori bulunamadı.";
            return RedirectToAction("CategoryList");
        }


        return View(result.Data); // CategoryDetailDto gibi bir model dönmeli
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CategoryDeleteConfirmed(int id)
    {
        var result = await _categoryService.DeleteAsync(id);

        if (!result.Success)
        {
            TempData["Error"] = result.Message ?? "Silme işlemi başarısız.";
        }

        return RedirectToAction("CategoryList");
    }

    [HttpGet]
    public async Task<IActionResult> CategoryEdit(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        if (result.Success) 
        {
            TempData["Error"] = result.Message ?? "Kategori bulunamadı";
        }
        var categoryUpdateDto = new CategoryUpdateDto
        {
            Id = id,
            CategoryName = result.Data.CategoryName,
            Description = result.Data.Description
        };

        return View(categoryUpdateDto);
    }

    [HttpPost]
    public async Task<IActionResult> CategoryEdit(CategoryUpdateDto categoryUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return View(categoryUpdateDto);
        }

        var result = await _categoryService.UpdateAsync(categoryUpdateDto);

        if (!result.Success) {

            ViewBag.Error = result.Message;
            return View(categoryUpdateDto);
        }

        return RedirectToAction("CategoryList");

    }
}
