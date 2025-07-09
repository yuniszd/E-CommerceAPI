namespace E_CommerceAPI.Application.DTOs.CategoryDTOs;

public record CategoryCreateDto
{
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
}