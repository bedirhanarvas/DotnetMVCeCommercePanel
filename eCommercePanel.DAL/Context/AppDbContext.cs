using eCommercePanel.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace eCommercePanel.DAL.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected AppDbContext()
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        

        modelBuilder.Entity<Role>()
        .HasIndex(r => r.RoleType)
        .IsUnique(); // Aynı RoleType tekrar eklenmesini engeller

        // User - Role İlişkisi
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Sadece 2 rol ekle (Id sabit)
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, RoleType = "Customer" },
            new Role { Id = 2, RoleType = "Admin" }
        );

        // Order - OrderItem ilişkisi
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        // OrderItem - Product ilişkisi
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems) // Product sınıfında OrderItems koleksiyonu var
            .HasForeignKey(oi => oi.ProductId);

        // User - Address ilişkisi
        modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId);

        // Product - Category ilişkisi
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        // Order - User ilişkisi
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Döngüsel silme sorununu engellemek için

        // Order - Address ilişkisi
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Address)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.AddressId)
            .OnDelete(DeleteBehavior.Restrict); // Döngüsel silme sorununu engellemek için
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=testFaruk;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

}
