using E_CommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceAPI.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasOne(o => o.Buyer)
               .WithMany(u => u.Orders)
               .HasForeignKey(o => o.BuyerId);

        builder.HasOne(o => o.Product)
               .WithMany(p => p.Orders)
               .HasForeignKey(o => o.ProductId);
    }
}
