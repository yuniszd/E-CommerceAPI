namespace E_CommerceAPI.Application.DTOs.OrderDTOs;

public record OrderResponseDto
{
    public Guid Id { get; set; }
    public string BuyerId { get; set; }
    public string BuyerName { get; set; }
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
}
