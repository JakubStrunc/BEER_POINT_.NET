using Microsoft.EntityFrameworkCore;
using PNET_semestralka.Data;
using PNET_semestralka.Models;

namespace PNET_semestralka.Services
{
	public class ProductService
	{
		private readonly ApplicationDbContext _context;

		public ProductService(ApplicationDbContext context)
		{
			_context = context;
		}

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

	}
}
