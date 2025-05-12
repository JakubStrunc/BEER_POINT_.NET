namespace PNET_semestralka.DataDto
{

	/// <summary>
	/// data transfer object used for updating the quantity of a product
	/// </summary>
	public class UpdateQuantityDto
	{
		public int ProductId { get; set; }
		public int NewQuantity { get; set; }
	}
}
