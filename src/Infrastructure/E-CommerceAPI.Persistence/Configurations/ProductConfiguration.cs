using E_CommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceAPI.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId);

        builder.HasOne(p => p.Owner)
               .WithMany(u => u.Products)
               .HasForeignKey(p => p.OwnerId);

        builder.Property(p => p.Price)
       .HasColumnType("decimal(18,2)");

    }
}
