using Ecom.core.Entites.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {

            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.HasData(
                new DeliveryMethod { Id = 1, Name = "Standard Delivery", Price = 5.00m, Description = "Delivered in 3-5 business days", DeliveryTime = "3-5 days" },
                new DeliveryMethod { Id = 2, Name = "Express Delivery", Price = 10.00m, Description = "Delivered in 1-2 business days", DeliveryTime = "1-2 days" },
                new DeliveryMethod { Id = 3, Name = "Next Day Delivery", Price = 20.00m, Description = "Delivered the next business day", DeliveryTime = "Next day" }
            );

        }
    }
}
