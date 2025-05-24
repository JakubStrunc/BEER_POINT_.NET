
using Microsoft.EntityFrameworkCore;
using PNET_semestralka.Models;


namespace PNET_semestralka.Data
{

	/// <summary>
	/// represents the application's database context with providing access to database tables for entities
	/// </summary>
	public class ApplicationDbContext : DbContext
    {

		#region Database tables
		public DbSet<User> Users{ get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<SendingAddress> SendingAddress { get; set; }

		#endregion

		/// <summary>
		/// initializes a new instance of the <see cref="ApplicationDbContext"/> class
		/// </summary>
		/// <param name="options">the options to configure the database context</param>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			// [Association] product belongs to a seller (1:N)
			modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(s => s.Products) 
                .HasForeignKey(p => p.SellerId);

			// [Aggregation] sending address belongs to a customer (1:N)
			modelBuilder.Entity<SendingAddress>()
                .HasOne(sd => sd.Customer)
                .WithMany(c => c.ShippingDetails)
                .HasForeignKey(sd => sd.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

			// [Association] order belongs to a customer (1:N)
			modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

			// [Composite Key] orderItem has unique combination of order and product
			modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ProductId });

			// [Aggregation] order aggregates order items (1:N)
			modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // delete orders items with deleting the order

			// [Association] order item references a product (1:1)
			modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.NoAction); // dont delete orders items with deleting the product

        }

		/// <summary>
		/// gets or sets the collection of customers
		/// </summary>
		public DbSet<PNET_semestralka.Models.Customer> Customer { get; set; } = default!;
    }
}




