using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestProject.Entities;

namespace TestProject.Database;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
        builder.Property(r => r.Price).HasColumnType("decimal(18,2)");
        builder.HasMany(r => r.AvailableServices).WithMany();
        IMutableNavigation? navigation = builder.Metadata.FindNavigation(nameof(Room.AvailableServices));
        if (navigation is not null)
        {
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}