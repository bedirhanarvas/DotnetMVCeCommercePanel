using eCommercePanel.BLL.Results;
using eCommercePanel.DAL.DTOs.ProductDTOs.Requests;
using eCommercePanel.DAL.DTOs.ProductDTOs.Responses;

namespace eCommercePanel.BLL.Services;

public interface IProductService
{
    Task<DataResult<List<GetAllProductsDto>>> GetAllAsync();
    Task<DataResult<ProductDetailDto>> GetByIdAsync(int id);
    Task<Result> AddAsync(ProductCreateDto productCreateDto);
    Task<Result> UpdateAsync(ProductUpdateDto productUpdateDto);
    Task<Result> DeleteAsync(int id);
}
