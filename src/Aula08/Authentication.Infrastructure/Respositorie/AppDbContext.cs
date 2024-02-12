using Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Respositorie
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);            

           // modelBuilder.Entity<User>( u =>{
           //     u.ToTable("Users");
           //     u.HasKey(x => x.Id);
           //     u.Property(u => u.Role).IsRequired();
           //     u.Property(u => u.UserName).IsRequired();
           //     u.Property(u => u.Email).IsRequired();
           //     u.Property(u => u.Password).IsRequired();
           // });

           // modelBuilder.Entity<Product>(p =>
           //{
           //    p.ToTable("Products");
           //    p.HasKey(x => x.Id);
           //    p.Property(p => p.Category).IsRequired();
           //});
        }
    }
}
