﻿using Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Order = Domain.Models.Orders.Order;

namespace Persistence.Data.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.Property(d => d.SubTotal).HasColumnType("decimal(8,2)");
            builder.HasMany(o => o.Items)
                .WithOne();
            builder.OwnsOne(o => o.Address, o => o.WithOwner());
        }
    }
}
