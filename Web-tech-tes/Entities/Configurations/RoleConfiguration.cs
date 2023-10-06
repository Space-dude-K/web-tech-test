using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(p => p.Name)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired();

            builder
                .HasKey(p => p.Id)
                .HasName("PK_Role");

            builder.HasData(
                new Role() { Id = 1, Name = "User" },
                new Role() { Id = 2, Name = "Admin" },
                new Role() { Id = 3, Name = "Support" },
                new Role() { Id = 4, Name = "SuperAdmin" }
                );
        }
    }
}