using E_CommerceAPI.Application.DTOs.CategoryDTOs;
using E_CommerceAPI.Domain.Entities;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface ICategoryService
{
    Task<List<CategoryResponseDto>> GetAllCategoriesAsync();
    Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreateDto dto);
}
