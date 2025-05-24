namespace PNET_semestralka.DataDto
{

	/// <summary>
	/// data transfer object representing product's details
	/// </summary>
	public class ProductDto
	{
		public int Id { get; set; }
		public string Nazev { get; set; }
		public string Popis { get; set; }
		public int Cena { get; set; }
		public int Mnozstvi { get; set; }
		public string Image { get; set; }
		public IFormFile FileToUpload { get; set; }

		public bool UseExistingImage { get; set; }
	}



}
