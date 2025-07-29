using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPI.Application.DTOs.ProductDTOs;

public class ProductCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Şəkil mütləqdir")]
    public IFormFile Image { get; set; } = null!;
}
