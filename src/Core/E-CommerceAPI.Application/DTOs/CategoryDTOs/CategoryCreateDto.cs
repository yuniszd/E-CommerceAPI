namespace E_CommerceAPI.Application.DTOs.CategoryDTOs;

public record CategoryCreateDto
{
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
}