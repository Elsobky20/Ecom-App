using Ecom.core.Entites.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    public class OrderConfiguration: IEntityTypeConfiguration<Orders>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Orders> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, sa => {sa.WithOwner(); });
            builder.HasMany(x=>x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(o => o.status).HasConversion(o => o.ToString(),
                o => (Status)Enum.Parse(typeof(Status), o));
            builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");
        }
    }
    
        
}
