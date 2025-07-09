namespace E_CommerceAPI.Domain.Entities;

public class OrderProduct
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }  
    public decimal PriceAtPurchase { get; set; }  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
}
