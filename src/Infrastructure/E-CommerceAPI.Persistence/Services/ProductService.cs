using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.DTOs.ProductDTOs;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetUserId() =>
        _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    private bool IsSeller() =>
        _httpContextAccessor.HttpContext.User.IsInRole("Seller");

    public async Task<List<ProductResponseDto>> GetAllAsync(Guid? categoryId, decimal? minPrice, decimal? maxPrice, string search)
    {
        var query = _context.Products
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Include(p => p.Owner)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Title.Contains(search) || p.Description.Contains(search));

        var products = await query.ToListAsync();

        return products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Price = p.Price,
            ImageUrls = p.Images.Select(i => i.ImageUrl).ToList(),
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name,
            OwnerId = p.OwnerId,
            OwnerName = p.Owner?.UserName
        }).ToList();
    }

    public async Task<ProductResponseDto> GetByIdAsync(Guid id)
    {
        var p = await _context.Products
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (p == null) return null;

        return new ProductResponseDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Price = p.Price,
            ImageUrls = p.Images.Select(i => i.ImageUrl).ToList(),
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name,
            OwnerId = p.OwnerId,
            OwnerName = p.Owner?.UserName
        };
    }

    public async Task<bool> CreateAsync(ProductCreateDto dto)
    {
        if (!IsSeller()) return false;

        var userId = GetUserId();

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            OwnerId = userId,
            Images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = dto.ImageUrl }
            }
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, ProductCreateDto dto)
    {
        var userId = GetUserId();

        var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return false;

        if (product.OwnerId != userId) return false;

        product.Title = dto.Title;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;

        // Şəkilləri silib tək yeni şəkil əlavə et
        product.Images.Clear();
        product.Images.Add(new ProductImage { ImageUrl = dto.ImageUrl });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var userId = GetUserId();

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return false;

        if (product.OwnerId != userId) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductResponseDto>> GetMyProductsAsync()
    {
        var userId = GetUserId();

        var products = await _context.Products
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Where(p => p.OwnerId == userId)
            .ToListAsync();

        return products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Price = p.Price,
            ImageUrls = p.Images.Select(i => i.ImageUrl).ToList(),
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name,
            OwnerId = p.OwnerId,
            OwnerName = p.Owner?.UserName
        }).ToList();
    }

    public async Task<ProductResponseDto> GetProductByIdAsync(Guid id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return null;

        return new ProductResponseDto
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            OwnerId = product.OwnerId,
            OwnerName = product.Owner?.UserName,
            ImageUrls = product.Images.Select(i => i.ImageUrl).ToList()
        };
    }
}
