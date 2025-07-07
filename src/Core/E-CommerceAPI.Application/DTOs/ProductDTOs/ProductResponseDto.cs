namespace E_CommerceAPI.Application.DTOs.ProductDTOs;

public record ProductResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<string> ImageUrls { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string OwnerId { get; set; }
    public string OwnerName { get; set; }
}
