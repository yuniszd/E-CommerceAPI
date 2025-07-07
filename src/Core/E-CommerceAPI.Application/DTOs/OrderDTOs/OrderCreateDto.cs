namespace E_CommerceAPI.Application.DTOs.OrderDTOs;

public record OrderCreateDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
