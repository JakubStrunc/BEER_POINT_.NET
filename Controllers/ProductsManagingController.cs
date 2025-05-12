using Microsoft.AspNetCore.Mvc;
using PNET_semestralka.Data;
using PNET_semestralka.DataDto;
using PNET_semestralka.Migrations;
using PNET_semestralka.Models;

namespace PNET_semestralka.Controllers
{
    public class ProductsManagingController : Controller
    {
        private MyDatabase _database;
        public ProductsManagingController(ApplicationDbContext context)
        {
            _database = new MyDatabase(context);
        }

        public IActionResult Index()
        {

            var products = _database.GetAllProducts();
            return View(products);
        }

		[HttpPost]
		public IActionResult DeleteProduct([FromBody] IdDto data)
		{
			var product = _database.GetProductById(data.Id);
			if (product == null)
				return BadRequest("Produkt nenalezen");

			_database.DeleteProduct(product);
			return Ok();
		}

		[HttpPost]
		public IActionResult AddProduct([FromForm] ProductDto data)
		{
			var errors = new Dictionary<string, string>();

			// Validace
			if (string.IsNullOrWhiteSpace(data.Nazev))
				errors["productName"] = "Jméno produktu je povinné.";

			if (string.IsNullOrWhiteSpace(data.Popis))
				errors["productDescription"] = "Popis je povinný.";
			else if (data.Popis.Length > 60)
				errors["productDescription"] = "Popis nesmí být delší než 60 znaků.";

			if (data.Cena <= 0)
				errors["productPrice"] = "Cena musí být větší než 0.";

			if (data.Mnozstvi < 0)
				errors["productStock"] = "Množství musí být nezáporné.";

			if (data.FileToUpload == null || data.FileToUpload.Length == 0)
				errors["fileToUpload"] = "Obrázek je povinný.";

			if (errors.Any())
				return Json(new { success = false, errors });

			// Uložení souboru
			string fileName = Path.GetFileName(data.FileToUpload.FileName);
			string savePath = Path.Combine("wwwroot/img/produkty", fileName);
			using (var stream = new FileStream(savePath, FileMode.Create))
			{
				data.FileToUpload.CopyTo(stream);
			}

			// Získání přihlášeného prodejce
			var email = HttpContext.Session.GetString("Email");
			var user = _database.GetUserByEmail(email);

			if (user is not Seller seller)
				return BadRequest("Chyba v DB");

			// Vložení do databáze
			_database.AddNewProduct(data.Nazev, data.Popis, data.Cena, data.Mnozstvi, seller, fileName);

			return Json(new { success = true });
		}

		[HttpGet]
		public IActionResult GetProduct(int id)
		{
			var product = _database.GetProductById(id);
			if (product == null) return NotFound();

			return Json(new
			{
				id = product.Id,
				nazev = product.Nazev,
				popis = product.Popis,
				cena = product.Cena,
				mnozstvi = product.Mnozstvi,
				photo = product.Photo
			});
		}

		[HttpPost]
		public IActionResult EditProduct([FromForm] ProductDto data)
		{
			var errors = new Dictionary<string, string>();

			// Validace
			if (string.IsNullOrWhiteSpace(data.Nazev))
				errors["productName"] = "Jméno produktu je povinné.";
			if (string.IsNullOrWhiteSpace(data.Popis))
				errors["productDescription"] = "Popis je povinný.";
			else if (data.Popis.Length > 60)
				errors["productDescription"] = "Popis nesmí být delší než 60 znaků.";
			if (data.Cena <= 0)
				errors["productPrice"] = "Cena musí být větší než 0.";
			if (data.Mnozstvi < 0)
				errors["productStock"] = "Množství musí být nezáporné.";
			if (!data.UseExistingImage && (data.FileToUpload == null || data.FileToUpload.Length == 0))
				errors["fileToUpload"] = "Musíte vybrat nový obrázek nebo použít původní.";

			if (errors.Any())
				return Json(new { success = false, errors });

			// Zpracování souboru
			string fileName = null;
			if (!data.UseExistingImage && data.FileToUpload != null)
			{
				fileName = Path.GetFileName(data.FileToUpload.FileName);
				string savePath = Path.Combine("wwwroot/img/produkty", fileName);
				using (var stream = new FileStream(savePath, FileMode.Create))
				{
					data.FileToUpload.CopyTo(stream);
				}
			}

			// Volání metody ve stylu AddNewProduct
			_database.UpdateProduct(data.Id, data.Nazev, data.Popis, data.Cena, data.Mnozstvi, data.UseExistingImage, fileName);

			return Json(new { success = true });
		}


	}
}
