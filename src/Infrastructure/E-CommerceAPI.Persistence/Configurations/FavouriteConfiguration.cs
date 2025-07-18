﻿using E_CommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_CommerceAPI.Persistence.Configurations;

public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    public void Configure(EntityTypeBuilder<Favourite> builder)
    {
        builder.HasKey(f => new { f.UserId, f.ProductId });

        builder.HasOne(f => f.User)
               .WithMany(u => u.Favourites)
               .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Product)
               .WithMany(p => p.Favourites)
               .HasForeignKey(f => f.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
