using E_CommerceAPI.Domain.Entities;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProducts(int? categoryId, decimal? minPrice, decimal? maxPrice, string search);
    Task<Product?> GetProductById(int id);
    Task<Product> CreateProduct(Product product);
    Task<bool> UpdateProduct(int id, Product updatedProduct);
    Task<bool> DeleteProduct(int id);
    Task<List<Product>> GetMyProducts();
}
