﻿namespace E_CommerceAPI.Domain.Entities;

public class Category : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; } 
    public Category ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; }
    public ICollection<Product> Products { get; set; }
}
