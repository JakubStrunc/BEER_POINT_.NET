using PNET_semestralka.Migrations;
using PNET_semestralka.Services;

namespace PNET_semestralka.CommandDesignPatern
{

	/// <summary>
	/// command for updating the quantity of a product in the customer's cart
	/// </summary>
	public class UpdateQuantityCommand : ICommand
	{
		private readonly CartService _cartService;
		private readonly int _customerId;
		private readonly int _productId;
		private readonly int _newQuantity;

		public UpdateQuantityCommand(CartService cartService, int customerId, int productId, int newQuantity)
		{
			_cartService = cartService;
			_customerId = customerId;
			_productId = productId;
			_newQuantity = newQuantity;
		}

		/// <summary>
		/// executes the command to update the quantity of a product in the customer's cart
		/// </summary>
		/// <exception cref="Exception">thrown when the cart is not found or the product is not in the cart</exception>
		public void Execute()
		{
			var order = _cartService.GetCartForCustomer(_customerId);
			if (order == null)
				throw new Exception("Košík nenalezen");

			var item = order.OrderItems.FirstOrDefault(i => i.Product.Id == _productId);
			if (item == null)
				throw new Exception("Produkt není v košíku");

			item.Pocet = _newQuantity;
			_cartService.Save();
		}
	}
}
