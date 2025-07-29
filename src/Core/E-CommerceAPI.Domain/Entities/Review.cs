namespace E_CommerceAPI.Domain.Entities;

public class Review : BaseEntity
{
    public Guid Id { get; set; }
    public string? Comment { get; set; }
    public int Rating { get; set; } 
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
