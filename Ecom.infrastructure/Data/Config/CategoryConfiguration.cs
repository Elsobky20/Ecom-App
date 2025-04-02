using Ecom.core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Ecom.infrastructure.Data.Config
{
    class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
       public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);

            builder.Property(p => p.Id).IsRequired() ;
            builder.HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic Items" },
                new Category { Id = 2, Name = "Clothes", Description = "Clothes Items" }
                );


        }
    }
}
