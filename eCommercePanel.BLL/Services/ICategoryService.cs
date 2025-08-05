using eCommercePanel.BLL.Results;
using eCommercePanel.DAL.DTOs.CategoryDTOs.Requests;
using eCommercePanel.DAL.DTOs.CategoryDTOs.Response;

namespace eCommercePanel.BLL.Services
{
    public interface ICategoryService
    {
        Task<DataResult<List<GetAllCategoriesDto>>> GetAllAsync();
        Task<DataResult<CategoryDetailDto>> GetByIdAsync(int id);
        Task<Result> AddAsync(CategoryCreateDto categoryCreateDto);
        Task<Result> UpdateAsync(CategoryUpdateDto categoryUpdateDto);
        Task<Result> DeleteAsync(int id);
    }
}
