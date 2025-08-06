using eCommercePanel.BLL.Results;
using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.CategoryDTOs.Requests;
using eCommercePanel.DAL.DTOs.CategoryDTOs.Response;
using eCommercePanel.DAL.Entities;
using eCommercePanel.DAL.Interfaces;

namespace eCommercePanel.BLL.Managers;

public class CategoryManager : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryManager(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<Result> AddAsync(CategoryCreateDto categoryCreateDto)
    {
        var addCategory = new Category()
        {
            CategoryName = categoryCreateDto.CategoryName,
            Description = categoryCreateDto.Description

        };
        await _categoryRepository.AddCategoryAsync(addCategory);
        await _categoryRepository.SaveAsync();

        return new SuccessResult(addCategory + " başarıyla kaydedildi.");
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {

            return new ErrorResult("Silmek istediğiniz kategori bulunamadı.");
        }

        _categoryRepository.DeleteCategory(category);
        await _categoryRepository.SaveAsync();

        return new SuccessResult("Başarıyla silindi.");
    }

    public async Task<DataResult<List<GetAllCategoriesDto>>> GetAllAsync()
    {
        var category = await _categoryRepository.GetAllAsync();

        var categoryDto = category.Select(c => new GetAllCategoriesDto
        {
            Id = c.Id,
            CategoryName = c.CategoryName,
        }).ToList();

        return new SuccessDataResult<List<GetAllCategoriesDto>>(categoryDto, "Ürünler başarıyla getirildi.");
    }

    public async Task<DataResult<CategoryDetailDto>> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {

            return new ErrorDataResult<CategoryDetailDto>(null, "Bu kategori bulunamadı.");
        }

        var dto = new CategoryDetailDto()
        {
            Id=category.Id,
            CategoryName = category.CategoryName,
        };

        return new SuccessDataResult<CategoryDetailDto>(dto, "Kategori başarıyla getirildi.");
    }

    public async Task<Result> UpdateAsync(CategoryUpdateDto categoryUpdateDto)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryUpdateDto.Id);

        if (category == null)
        {
            return new ErrorResult("Bu kategori bulunamadı.");
        }

        if (!string.IsNullOrEmpty(categoryUpdateDto.CategoryName))
        {
            category.CategoryName = categoryUpdateDto.CategoryName;
        }

        if (!string.IsNullOrEmpty(categoryUpdateDto.Description))
        {
            category.Description = categoryUpdateDto.Description;
        }

        _categoryRepository.Update(category);
        await _categoryRepository.SaveAsync();

        return new SuccessResult("Başarıyla güncellendi.");
    }
}
