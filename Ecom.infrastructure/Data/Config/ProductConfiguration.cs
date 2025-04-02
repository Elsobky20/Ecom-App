using Microsoft.EntityFrameworkCore;
using Ecom.core.Entites.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired();
            builder.HasData(
                new Product { Id = 1, Name = "Laptop", Description = "Dell Laptop", Price = 50000, CategoryId = 1 },
                new Product { Id = 2, Name = "Mobile", Description = "Samsung Mobile", Price = 20000, CategoryId = 1 },
                new Product { Id = 3, Name = "Shirt", Description = "Peter England Shirt", Price = 1000, CategoryId = 2 },
                new Product { Id = 4, Name = "T-Shirt", Description = "Polo T-Shirt", Price = 500, CategoryId = 2 }
                );
        }
    }
}
