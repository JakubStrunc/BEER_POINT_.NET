using PNET_semestralka.Migrations;

namespace PNET_semestralka.CommandDesignPatern
{
	/// <summary>
	/// command for removing a product from the customer's cart
	/// </summary>
	public class RemoveProductCommand : ICommand
	{
		private readonly MyDatabase _database;
		private readonly int _customerId;
		private readonly int _productId;

		public RemoveProductCommand(MyDatabase database, int customerId, int productId)
		{
			_database = database;
			_customerId = customerId;
			_productId = productId;
		}

		/// <summary>
		/// rxecutes the command to remove the product from the customer's cart
		/// </summary>
		/// <exception cref="Exception">thrown when the cart is not found or the product is not in the cart</exception>
		public void Execute()
		{
			var cart = _database.GetCartForCustomer(_customerId);
			if (cart == null) throw new Exception("Košík nenalezen");

			var item = cart.OrderItems.FirstOrDefault(i => i.Product.Id == _productId);
			if (item == null) throw new Exception("Produkt není v košíku");

			cart.OrderItems.Remove(item);

			_database.Save();
		}
	}
}
