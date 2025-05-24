namespace PNET_semestralka.DataDto
{

	/// <summary>
	/// data transfer object used for updating status of order
	/// </summary>
	public class UpdateOrderStatusDto
	{
		public int Id { get; set; }
		public string Stav { get; set; }
	}

}
