namespace E_CommerceAPI.Domain.Entities;

    public class Product
    {
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public string OwnerId { get; set; }
    public Category? Category { get; set; }
    public AppUser Owner { get; set; }
    public ICollection<ProductImage> Images { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Favourite> Favourites { get; set; } 
    public ICollection<Order> Orders { get; set; }

}

