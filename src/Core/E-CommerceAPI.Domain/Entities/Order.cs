namespace E_CommerceAPI.Domain.Entities;

public class Order : BaseEntity
{
    public Guid? Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string? BuyerId { get; set; }
    public AppUser? Buyer { get; set; }
    public Guid? ProductId { get; set; }
    public Product Product { get; set; }
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
    public OrderStatus Status { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }
    public decimal TotalPrice
    {
        get
        {
            if (OrderProducts == null)
                return 0;

            return OrderProducts.Sum(op => op.Quantity * op.PriceAtPurchase);
        }
    }
}

