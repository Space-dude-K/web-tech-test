using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired();
            builder.Property(p => p.Name)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired();
            builder.Property(p => p.Email)
                .HasColumnType("NVARCHAR")
                .IsRequired();
            builder.Property(p => p.Age)
                .HasColumnType("INTEGER")
                .IsRequired();

            builder
                .HasKey(p => p.Id)
                .HasName("PK_User");

            builder
                .HasMany(p => p.Roles)
                .WithMany(p => p.Users);
        }
    }
}