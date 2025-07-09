namespace E_CommerceAPI.Domain.Entities;

public class Favourite : BaseEntity
{
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
}



