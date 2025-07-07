namespace E_CommerceAPI.Application.DTOs.CategoryDTOs;

public class CategoryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<CategoryResponseDto>? SubCategories { get; set; }
}
