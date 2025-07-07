using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.DTOs.CategoryDTOs;
using E_CommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceAPI.Persistence.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryResponseDto>> GetAllCategoriesAsync()
    {
        var categories = await _context.Categories
            .Include(c => c.SubCategories)
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync();

        // Manual mapping
        return categories.Select(c => MapToResponseDto(c)).ToList();
    }

    public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            ParentCategoryId = dto.ParentCategoryId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return MapToResponseDto(category);
    }

    private CategoryResponseDto MapToResponseDto(Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            SubCategories = category.SubCategories?.Select(sc => MapToResponseDto(sc)).ToList()
        };
    }
}
