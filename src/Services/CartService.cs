using Microsoft.EntityFrameworkCore;
using PNET_semestralka.Data;
using PNET_semestralka.FactoryDesignPatern;
using PNET_semestralka.Models;

namespace PNET_semestralka.Services
{
	public class CartService
	{
		private readonly ApplicationDbContext _context;

		public CartService(ApplicationDbContext context)
		{
			_context = context;
		}


		/// <summary>
		/// returns the cart for a customer if it exists
		/// </summary>
		/// <param name="customerId">the ID of the customer</param>
		/// <returns>the cart order for the customer or null if not found</returns>
		public Order? GetCartForCustomer(int customerId)
		{
			return _context.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.FirstOrDefault(o => o.CustomerId == customerId && o.Stav == "Košík");
		}

		/// <summary>
		/// creates a new cart for a customer
		/// </summary>
		/// <param name="customerId">the ID of the customer</param>
		/// <returns>the newly created cart</returns>
		public Order AddCartForCustomer(int customerId)
		{
			var customer = _context.Users.OfType<Customer>().FirstOrDefault(c => c.Id == customerId);

			var cart = new Order
			{
				Customer = customer,
				Stav = "Košík",
				OrderItems = new List<OrderItem>()
			};

			_context.Orders.Add(cart);
			_context.SaveChanges();

			return cart;
		}

		/// <summary>
		/// removes a product from the cart
		/// </summary>
		/// <param name="order">the order to remove</param>
		public void RemoveProductFromCart(Order order)
		{
			_context.Orders.Remove(order);
		}
		/// <summary>
		/// xhecks if a specific product is already in cart 
		/// </summary>
		/// <param name="cartId">ID of the cart to check</param>
		/// <param name="productId"> ID of the product to check for</param>
		/// <returns>True if the product is in the cart; otherwise false.</returns>
		public bool IsProductInCart(int cartId, int productId)
		{
			return _context.OrderItem.Any(oi =>
				oi.Order.Id == cartId &&
				oi.Product.Id == productId);
		}

		/// <summary>
		/// adds a product to cart with a default quantity 
		/// </summary>
		/// <param name="cartId">ID of the cart </param>
		/// <param name="productId">ID of the product</param>
		public void AddProductToCart(int cartId, int productId)
		{
			var cart = _context.Orders
				.Include(o => o.OrderItems)
				.FirstOrDefault(o => o.Id == cartId);

			var product = _context.Products.FirstOrDefault(p => p.Id == productId);

			OrderItemCreator itemFactory = new StandardOrderItemCreator();

			var newItem = itemFactory.Create(product);

			cart.OrderItems.Add(newItem);

			_context.SaveChanges();
		}

		/// <summary>
		/// saves changes to the context
		/// </summary>
		public void Save()
		{
			_context.SaveChanges();
		}

	}
}
