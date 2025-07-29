namespace E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; }
    public ICollection<Product> Products { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Favourite> Favourites { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }

    public AppUser()
    {
        Products = new List<Product>();
        Orders = new List<Order>();
        Favourites = new List<Favourite>();
        Reviews = new List<Review>();
        RefreshTokens = new List<RefreshToken>();
    }
}
