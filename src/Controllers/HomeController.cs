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
using PNET_semestralka.Services;

namespace PNET_semestralka.Controllers
{
	public class HomeController : Controller
	{

		private readonly UserService _userService;
		private readonly CartService _cartService;
		private readonly ProductService _productService;
		private readonly PasswordHasher<User> _hasher;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_userService = new(context);
			_cartService = new(context);
			_productService = new(context);
			_hasher = new PasswordHasher<User>();
		}


		public IActionResult Index()
		{
			var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";


			if (isLoggedIn)
			{
				var email = HttpContext.Session.GetString("Email");

				var user = _userService.GetUserByEmail(email);

				if (user is Customer)
				{
					Console.WriteLine(user.Id);
					Console.WriteLine(user.Email);

					var cart = _cartService.GetCartForCustomer(user.Id);

					// Pokud neexistuje, vytvoøíme nový
					if (cart == null)
					{
						_cartService.AddCartForCustomer(user.Id);
					}
				}

			}
			var products = _productService.GetAllProducts();
			return View(products);
		}

		[HttpPost]
		public IActionResult Login([FromBody] UserRequestDto data)
		{
			var user = _userService.GetUserByEmail(data.Email);
			if (user == null)
				return BadRequest("Uživatel nenalezen");

			var result = _hasher.VerifyHashedPassword(user, user.Heslo, data.Password);
			if (result != PasswordVerificationResult.Success)
				return BadRequest("Špatné heslo");

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
				return Json(new { success = false, message = "Všechna pole jsou povinná." });
			}

			if (_userService.GetUserByEmail(data.Email) != null)
			{
				return Json(new { success = false, message = "Email již existuje." });
			}

			_userService.AddCustomer(data.Username, data.Email, data.Password);

			// Pøihlášení
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
			var email = HttpContext.Session.GetString("Email"); // pozor: možná vhodnìjší název by byl "UserEmail"


			var user = _userService.GetUserByEmail(email);


			var cart = _cartService.GetCartForCustomer(user.Id);


			// Zkontrolovat, jestli už produkt je v košíku
			if (_cartService.IsProductInCart(cart.Id, data.Id))
			{
				return Json(new { success = false }); // už tam je
			}

			// Pøidat produkt
			_cartService.AddProductToCart(cart.Id, data.Id);
			return Json(new { success = true });
		}
	}
}