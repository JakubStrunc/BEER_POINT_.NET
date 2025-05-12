using PNET_semestralka.Models;

namespace PNET_semestralka.FactoryDesignPatern
{
	/// <summary>
	/// concrete implementation of the OrderItemCreator that creates standard order items with a default quantity of 1
	/// </summary>
	public class StandardOrderItemCreator : OrderItemCreator
	{
		/// <summary>
		/// creates an OrderItem with the provided product and sets the quantity to 1
		/// </summary>
		/// <param name="product">the product</param>
		/// <returns>a new OrderItem</returns>
		public override OrderItem Create(Product product)
		{
			return new OrderItem { Product = product, Pocet = 1 };
		}
	}

}
