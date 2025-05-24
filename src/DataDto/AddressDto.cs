namespace PNET_semestralka.DataDto
{
	/// <summary>
	/// data transfer object representing an address
	/// </summary>
	public class AddressDto
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Street { get; set; }
		public int HouseNumber { get; set; }
		public int OrientationNumber { get; set; }
		public int PostalCode { get; set; }
		public string City { get; set; }
	}




}
