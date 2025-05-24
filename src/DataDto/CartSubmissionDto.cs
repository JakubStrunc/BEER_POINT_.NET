namespace PNET_semestralka.DataDto
{

	/// <summary>
	/// data transfer object used for submitting order address details
	/// </summary>
	public class CartSubmissionDto
	{
		public int? orderId { get; set; }

		public string? firstName { get; set; }
		public string? lastName { get; set; }
		public string? street { get; set; }
		public int? houseNumber { get; set; }
		public int? orientationNumber { get; set; }
		public int? postalCode { get; set; }
		public string? city { get; set; }

		public int? existingAddressId { get; set; }
	}

}
