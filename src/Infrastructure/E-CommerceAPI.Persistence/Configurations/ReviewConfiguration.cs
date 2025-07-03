using E_CommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceAPI.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasOne(r => r.User)
               .WithMany()
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Product)
               .WithMany(p => p.Reviews)
               .HasForeignKey(r => r.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
