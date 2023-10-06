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

            builder
                .HasMany(p => p.Users)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId"),
                j => j
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId"));
        }
    }
}