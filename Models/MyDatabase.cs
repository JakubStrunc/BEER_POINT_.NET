using PNET_semestralka.Data;
using PNET_semestralka.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PNET_semestralka.DataDto;
using PNET_semestralka.FactoryDesignPatern;

namespace PNET_semestralka.Migrations
{
	public class MyDatabase
	{
		private readonly ApplicationDbContext _context;


		/// <summary>
		/// initializes new instance of MyDatabase clas
		/// </summary>
		/// <param name="context">the ApplicationDbContext instance used to interact with the database</param>

		public MyDatabase(ApplicationDbContext context)
		{
			_context = context;
		}

		#region User Functions

		/// <summary>
		/// returns a user by their email address
		/// </summary>
		/// <param name="Email">the email address of the user</param>
		/// <returns>a user with the specified email or null if not found</returns>
		public User? GetUserByEmail(string Email)
		{
			return _context.Users.FirstOrDefault(u => u.Email == Email);
		}



		/// <summary>
		/// adds a new customer to the database
		/// </summary>
		/// <param name="username">the username of the customer.</param>
		/// <param name="email">the email address of the customer</param>
		/// <param name="password">the password for the customer account</param>
		public void AddCustomer(string username, string email, string password)
		{
			var customer = new Customer
			{
				UzivatelskeJmeno = username,
				Email = email
			};

			var hasher = new PasswordHasher<User>();
			customer.Heslo = hasher.HashPassword(customer, password);

			_context.Users.Add(customer); // EF recognizes it's a Customer
			_context.SaveChanges();
		}

		#endregion

		#region Order Functions

		/// <summary>
		/// returns a list of orders for a given customer
		/// </summary>
		/// <param name="customerId">the ID of the customer</param>
		/// <returns>a list of orders associated to the customer</returns>
		public List<Order> GetOrdersByCustomerId(int customerId)
		{
			return _context.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.Where(o => o.CustomerId == customerId && o.Stav != "Košík")
				.ToList();
		}

		/// <summary>
		/// returns a list of all orders
		/// </summary>
		/// <returns>a list of all orders</returns>
		public List<Order> GetAllOrders()
		{
			return _context.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.Where(o => o.Stav != "Košík")
				.ToList();
		}

		/// <summary>
		/// returns a specific order by ID
		/// </summary>
		/// <param name="id">the ID of the order</param>
		/// <returns>the order with the specified ID or null if not found</returns>
		public Order? GetOrderById(int id)
		{
			return _context.Orders.FirstOrDefault(o => o.Id == id);
		}

		/// <summary>
		/// updates the state of an order
		/// </summary>
		/// <param name="order">the order to update</param>
		/// <param name="state">the new state of the order</param>
		public void UpdateOrderToState(Order order, string state)
		{
			order.Stav = state;
			_context.Orders.Update(order);
			_context.SaveChanges();
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

		#endregion

		#region Product Functions

		/// <summary>
		/// Retrieves all products from the database 
		/// </summary>
		/// <returns>A list of all products</returns>
		public List<Product> GetAllProducts()
		{
			return _context.Products.Include(p => p.Seller).ToList(); // Fetch products with their seller information
		}

		/// <summary>
		/// returns a product by ID
		/// </summary>
		/// <param name="id">the ID of the product</param>
		/// <returns>the product with the specified ID or null if not found</returns>
		public Product? GetProductById(int id)
		{
			return _context.Products.FirstOrDefault(p => p.Id == id);
		}

		/// <summary>
		/// adds a new product
		/// </summary>
		/// <param name="nazev">the name of the product</param>
		/// <param name="popis">the description of the product</param>
		/// <param name="cena">the price of the product</param>
		/// <param name="mnozstvi">the quantity of the product</param>
		/// <param name="seller">the seller of the product</param>
		/// <param name="fileName">the file name of the product photo</param>
		public void AddNewProduct(string nazev, string popis, int cena, int mnozstvi, Seller seller, string fileName)
		{
			var product = new Product
			{
				Nazev = nazev,
				Popis = popis,
				Cena = cena,
				Mnozstvi = mnozstvi,
				Photo = fileName,
				Seller = seller
			};

			_context.Products.Add(product);
			_context.SaveChanges();
		}

		/// <summary>
		/// deletes a product from the database
		/// </summary>
		/// <param name="product">the product to delete</param>
		public void DeleteProduct(Product product)
		{
			_context.Products.Remove(product);
			_context.SaveChanges();
		}

		/// <summary>
		/// updates an existing product's details
		/// </summary>
		/// <param name="id">the ID of the product to update</param>
		/// <param name="nazev">the new name of the product</param>
		/// <param name="popis">the new description of the product</param>
		/// <param name="cena">the new price of the product</param>
		/// <param name="mnozstvi">the new quantity of the product</param>
		/// <param name="useExistingImage">indicates if the existing image should be used</param>
		/// <param name="fileName">the new file name of the product photo</param>
		public void UpdateProduct(int id, string nazev, string popis, int cena, int mnozstvi, bool useExistingImage, string fileName = null)
		{
			var product = _context.Products.FirstOrDefault(p => p.Id == id);
			if (product == null) return;

			product.Nazev = nazev;
			product.Popis = popis;
			product.Cena = cena;
			product.Mnozstvi = mnozstvi;

			if (!useExistingImage && !string.IsNullOrEmpty(fileName))
			{
				if (!string.IsNullOrEmpty(product.Photo))
				{
					string oldPath = Path.Combine("wwwroot/img/produkty", product.Photo);
					if (System.IO.File.Exists(oldPath))
						System.IO.File.Delete(oldPath);
				}

				product.Photo = fileName;
			}

			_context.SaveChanges();
		}

		#endregion

		#region Address Functions

		/// <summary>
		/// adds a new address to the database
		/// </summary>
		/// <param name="address">the address to add</param>
		public void AddAddress(SendingAddress address)
		{
			_context.SendingAddress.Add(address);
			_context.SaveChanges();
		}

		/// <summary>
		/// returns a list of addresses for a given customer
		/// </summary>
		/// <param name="customerId">the ID of the customer</param>
		/// <returns>a list of addresses associated with the customer</returns>
		public List<AddressDto> GetAddressesById(int customerId)
		{
			return _context.SendingAddress
				.Where(a => a.CustomerId == customerId)
				.Select(a => new AddressDto
				{
					Id = a.Id,
					FirstName = a.Jmeno,
					LastName = a.Prijmeni,
					Street = a.Ulice,
					HouseNumber = a.PopisneCislo,
					OrientationNumber = a.OrientacniCislo,
					PostalCode = a.Psc,
					City = a.Mesto
				})
				.ToList();
		}

		/// <summary>
		/// retrieves an address by its ID and associated user ID
		/// </summary>
		/// <param name="addressId">the ID of the address to retrieve</param>
		/// <param name="userId">the ID of the user associated with the address</param>
		/// <returns>the address that matches the specified address ID and user ID, or null if not found</returns>
		public SendingAddress GetAddressByIdAndUser(int? addressId, int userId)
		{
			return _context.SendingAddress
				.FirstOrDefault(a => a.Id == addressId && a.CustomerId == userId);
		}

		/// <summary>
		/// updates the address information for a given order
		/// </summary>
		/// <param name="order">the order to update</param>
		/// <param name="address">the new address to assign to the order</param>
		public void UpdateOrderAddress(Order order, SendingAddress address)
		{
			order.Jmeno = address.Jmeno;
			order.Prijmeni = address.Prijmeni;
			order.Ulice = address.Ulice;
			order.PopisneCislo = address.PopisneCislo;
			order.OrientacniCislo = address.OrientacniCislo;
			order.Psc = address.Psc;
			order.Mesto = address.Mesto;

			_context.SaveChanges();
		}

		/// <summary>
		/// deletes a specified address from the database
		/// </summary>
		/// <param name="addressId">the ID of the address to delete</param>
		public void DeleteAddress(int addressId)
		{
			var address = _context.SendingAddress.Find(addressId);
			if (address != null)
			{
				_context.SendingAddress.Remove(address);
				_context.SaveChanges();
			}
		}

		#endregion

		#region Cart Function

		/// <summary>
		/// updates the stock quantities for the products
		/// </summary>
		/// <param name="items">the collection of order items whose product quantities need to be updated</param>
		public void UpdateStockQuantities(ICollection<OrderItem> items)
		{
			foreach (var item in items)
			{
				var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
				if (product != null)
				{
					product.Mnozstvi -= item.Pocet;
				}
			}
			_context.SaveChanges();
		}

		/// <summary>
		/// checks whether all products in the cart have sufficient stock
		/// </summary>
		/// <param name="cart">the cart to check.</param>
		/// <returns>True if all products have enough stock; false otherwise</returns>
		public bool AreAllProductsInStock(Order cart)
		{
			foreach (var item in cart.OrderItems)
			{
				if (item.Product.Mnozstvi < item.Pocet) 
					return false;
			}
			return true; 
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

		#endregion

		#region Utility Functions

		/// <summary>
		/// saves changes to the context
		/// </summary>
		public void Save()
		{
			_context.SaveChanges();
		}

		#endregion
	}
}
