using Entities.Configurations;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            var roles = new List<Role>() 
            { 
                new Role() { Id = 1, Name = "User" },
                new Role() { Id = 2, Name = "Admin" },
                new Role() { Id = 3, Name = "Support" },
                new Role() { Id = 4, Name = "SuperAdmin" }
            };
            modelBuilder.Entity<Role>().HasData(roles);

            var users = new List<User>()
            {
                new User() { Id = 1, Age = 20, Email = "test@mail.ru", Name = "Ivan" },
                new User() { Id = 2, Age = 22, Email = "test1@mail.ru", Name = "Petr" }
            };
            modelBuilder.Entity<User>().HasData(users);

            modelBuilder.Entity("UserRole").HasData
                (
                new { RoleId = 1, UserId = 1 },
                new { RoleId = 2, UserId = 1 },
                new { RoleId = 3, UserId = 1 },
                new { RoleId = 4, UserId = 1 },
                new { RoleId = 1, UserId = 2 }
                );
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}