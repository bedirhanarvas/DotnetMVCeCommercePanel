using eCommercePanel.BLL.Results;
using eCommercePanel.BLL.Services;
using eCommercePanel.DAL.DTOs.ProductDTOs.Requests;
using eCommercePanel.DAL.DTOs.ProductDTOs.Responses;
using eCommercePanel.DAL.Entities;
using eCommercePanel.DAL.Interfaces;

namespace eCommercePanel.BLL.Managers;

public class ProductManager : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductManager(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> AddAsync(ProductCreateDto productCreateDto)
    {
        var newProduct = new Product()
        {
            ProductName = productCreateDto.ProductName,
            Description = productCreateDto.Description,
            Price = productCreateDto.Price,
            Stock = productCreateDto.Stock,
            ImageUrl = productCreateDto.ImageUrl,
            CategoryId = productCreateDto.CategoryId
        };

        await _productRepository.AddAsync(newProduct);
        await _productRepository.SaveAsync();
        return new SuccessResult("Ürün başarıyla eklendi.");
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return new ErrorResult("Silinecek ürün bulunamadı.");
        }
        _productRepository.Delete(product);
        await _productRepository.SaveAsync();
        return new SuccessResult("Ürün başarıyla silindi.");
    }

    public async Task<DataResult<List<GetAllProductsDto>>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        var productDto = products.Select(p => new GetAllProductsDto
        {
            Id = p.Id,
            ProductName = p.ProductName,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            ImageUrl = p.ImageUrl,

        }).ToList();

        return new SuccessDataResult<List<GetAllProductsDto>>(productDto, "Ürünler başarıyla getirildi.");
    }

    public async Task<DataResult<ProductDetailDto>> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {

            return new ErrorDataResult<ProductDetailDto>(null, "Ürün bulunamadı.");

        }

        var dto = new ProductDetailDto
        {
            ProductName = product.ProductName,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl

        };
        return new SuccessDataResult<ProductDetailDto>(dto, "Ürün bulundu.");
    }

    public async Task<Result> UpdateAsync(ProductUpdateDto productUpdateDto)
    {
        var product = await _productRepository.GetByIdAsync(productUpdateDto.Id);
        if (product == null)
        {

            return new ErrorResult("Bu isimde bir ürün bulunmamaktadır.");
        }
        if (!string.IsNullOrEmpty(productUpdateDto.ProductName))
        {
            product.ProductName = productUpdateDto.ProductName;
        }
        if (!string.IsNullOrEmpty(productUpdateDto.Description))
        {
            product.Description = productUpdateDto.Description;
        }
        if (productUpdateDto.Price.HasValue)
        {
            product.Price = productUpdateDto.Price.Value;
        }
        if (productUpdateDto.Stock.HasValue)
        {
            product.Stock = productUpdateDto.Stock.Value;
        }
        if (!string.IsNullOrEmpty(productUpdateDto.ImageUrl))
        {
            product.ImageUrl = productUpdateDto.ImageUrl;

        }
        await _productRepository.Update(product);
        await _productRepository.SaveAsync();

        return new SuccessResult(product + " başarıyla güncellendi.");

    }
}
