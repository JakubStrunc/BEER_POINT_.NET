using PNET_semestralka.Models;

namespace PNET_semestralka.FactoryDesignPatern
{

	/// <summary>
	/// abstract class for creating OrderItems
	/// </summary>
	public abstract class OrderItemCreator
	{

		/// <summary>
		/// abstract method to create an OrderItem based on a provided product
		/// </summary>
		/// <param name="product">the product to be added to the order item</param>
		/// <returns>a new OrderItem associated with the provided product</returns>
		public abstract OrderItem Create(Product product);
	}

}
