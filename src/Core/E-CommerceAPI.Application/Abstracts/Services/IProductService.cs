using E_CommerceAPI.Application.DTOs.ProductDTOs;
using E_CommerceAPI.Domain.Entities;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllAsync(Guid? categoryId, decimal? minPrice, decimal? maxPrice, string search);
    Task<ProductResponseDto> GetProductByIdAsync(Guid id);
    Task<bool> CreateAsync(ProductCreateDto dto);
    Task<bool> UpdateAsync(Guid id, ProductCreateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<List<ProductResponseDto>> GetMyProductsAsync();
}