using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNET_semestralka.Data;
using PNET_semestralka.Migrations;
using PNET_semestralka.Models;
using PNET_semestralka.Data;
using PNET_semestralka.Migrations;
using PNET_semestralka.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using PNET_semestralka.DataDto;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace PNET_semestralka.Controllers
{
	public class HomeController : Controller
	{
		
		private readonly MyDatabase _database;
		private readonly PasswordHasher<User> _hasher;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_database = new MyDatabase(context);
			_hasher = new PasswordHasher<User>();
		}


		public IActionResult Index()
		{
			var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";


			if (isLoggedIn)
			{
				var email = HttpContext.Session.GetString("Email");

				var user = _database.GetUserByEmail(email);

				if (user is Customer)
				{
					Console.WriteLine(user.Id);
					Console.WriteLine(user.Email);

					var cart = _database.GetCartForCustomer(user.Id);

					// Pokud neexistuje, vytvo��me nov�
					if (_database.GetCartForCustomer(user.Id) == null)
					{
						_database.AddCartForCustomer(user.Id);
					}
				}

			}
			var products = _database.GetAllProducts();
			return View(products);
		}

		[HttpPost]
		public IActionResult Login([FromBody] UserRequestDto data)
		{
			var user = _database.GetUserByEmail(data.Email);
			if (user == null)
				return BadRequest("U�ivatel nenalezen");

			var result = _hasher.VerifyHashedPassword(user, user.Heslo, data.Password);
			if (result != PasswordVerificationResult.Success)
				return BadRequest("�patn� heslo");

			HttpContext.Session.SetString("IsLoggedIn", "true");
			HttpContext.Session.SetString("Role", user is Customer ? "Customer" : "Seller");
			HttpContext.Session.SetString("Email", user.Email);

			return Json(new { success = true });
		}

		[HttpPost]
		public IActionResult Register([FromBody] UserRequestDto data)
		{
			if (string.IsNullOrWhiteSpace(data.Username) ||
				string.IsNullOrWhiteSpace(data.Email) ||
				string.IsNullOrWhiteSpace(data.Password))
			{
				return Json(new { success = false, message = "V�echna pole jsou povinn�." });
			}

			if (_database.GetUserByEmail(data.Email) != null)
			{
				return Json(new { success = false, message = "Email ji� existuje." });
			}

			_database.AddCustomer(data.Username, data.Email, data.Password);

			// P�ihl�en�
			HttpContext.Session.SetString("IsLoggedIn", "true");
			HttpContext.Session.SetString("Email", data.Email);
			HttpContext.Session.SetString("Role", "Customer");

			return Json(new { success = true });
		}

		[HttpPost]
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();

			

			return Json(new { success = true });
		}

		[HttpPost]
		public IActionResult AddProduct([FromBody] IdDto data)
		{
			var email = HttpContext.Session.GetString("Email"); // pozor: mo�n� vhodn�j�� n�zev by byl "UserEmail"


			var user = _database.GetUserByEmail(email);


			var cart = _database.GetCartForCustomer(user.Id);


			// Zkontrolovat, jestli u� produkt je v ko��ku
			if (_database.IsProductInCart(cart.Id, data.Id))
			{
				return Json(new { success = false }); // u� tam je
			}

			// P�idat produkt
			_database.AddProductToCart(cart.Id, data.Id);
			return Json(new { success = true });
		}
	}
}