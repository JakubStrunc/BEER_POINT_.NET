using Microsoft.AspNetCore.Mvc;
using PNET_semestralka.CommandDesignPatern;
using PNET_semestralka.Data;
using PNET_semestralka.DataDto;
using PNET_semestralka.Migrations;
using PNET_semestralka.Models;
using PNET_semestralka.StrategyDesignPatern;
using PNET_semestralka.Services;

namespace PNET_semestralka.Controllers
{
    public class CartController : Controller
    {

		private readonly UserService _userService;
		private readonly OrderService _orderService;
		private readonly CartService _cartService;
		private readonly AddressService _addressService;
		private readonly ProductService _productService;
		public CartController(ApplicationDbContext context) 
        {
			_userService = new(context);
			_orderService = new(context);
			_cartService = new(context);
			_addressService = new(context);
			_productService = new(context);
		}
        public IActionResult Index()
        {
			var email = HttpContext.Session.GetString("Email");

			var user = _userService.GetUserByEmail(email);


			var cart = _cartService.GetCartForCustomer(user.Id);

            return View(cart);
        }

		[HttpPost]
		public IActionResult UpdateQuantity([FromBody] UpdateQuantityDto data)
		{
			var email = HttpContext.Session.GetString("Email");
			var user = _userService.GetUserByEmail(email);

			if (user is not Customer customer)
				return Unauthorized();

			try
			{
				var command = new UpdateQuantityCommand(_cartService, customer.Id, data.ProductId, data.NewQuantity);
				command.Execute();
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return BadRequest(new { success = false, message = ex.Message });
			}
		}

		[HttpPost]
		public IActionResult RemoveProduct([FromBody] IdDto data)
		{
			var email = HttpContext.Session.GetString("Email");
			var user = _userService.GetUserByEmail(email);
			if (user is not Customer customer)
				return Unauthorized();

			try
			{
				var command = new RemoveProductCommand(_cartService, customer.Id, data.Id);
				command.Execute();
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return BadRequest(new { success = false, message = ex.Message });
			}
		}

		[HttpGet]
		public JsonResult GetRecentAddresses()
		{
			var email = HttpContext.Session.GetString("Email");

			var user = _userService.GetUserByEmail(email);

			
			var addresses = _addressService.GetAddressesById(user.Id);

			
			return Json(addresses);
		}

		[HttpPost]
		public IActionResult SubmitOrder([FromBody] CartSubmissionDto address)
		{
			
			if (address.existingAddressId == null)
			{
				var errors = new Dictionary<string, string>();

				if (string.IsNullOrWhiteSpace(address.firstName))
					errors["firstName"] = "Jméno je povinné.";

				if (string.IsNullOrWhiteSpace(address.lastName))
					errors["lastName"] = "Příjmení je povinné.";

				if (string.IsNullOrWhiteSpace(address.street))
					errors["ulice"] = "Ulice je povinná.";

				if (address.orientationNumber == 0)
					errors["popisneCislo"] = "Číslo popisné je povinné.";

				if (address.postalCode == 0)
					errors["psc"] = "PSČ je povinné.";

				if (string.IsNullOrWhiteSpace(address.city))
					errors["mesto"] = "Město je povinné.";

				if (errors.Count > 0)
					return Json(new { success = false, errors });
			}

			var email = HttpContext.Session.GetString("Email");
			var user = _userService.GetUserByEmail(email);

			var cart = _cartService.GetCartForCustomer(user.Id);
			if (!_productService.AreAllProductsInStock(cart))
				return Json(new { success = false, error = "Některé produkty už nejsou skladem." });


			IStrategy strategy;

			if (address.existingAddressId != null)
			{
				strategy = new ExistingAddressStrategy(address.existingAddressId, _addressService);
			}
			else
			{
				
				var newAddress = new SendingAddress
				{
					Jmeno = address.firstName,
					Prijmeni = address.lastName,
					Ulice = address.street,
					PopisneCislo = address.houseNumber.Value,
					OrientacniCislo = address.orientationNumber.Value,
					Psc = address.postalCode.Value,
					Mesto = address.city,
					
				};

				strategy = new NewAddressStrategy(newAddress, _addressService);
			}

			var finalAddress = strategy.GetAddress(user.Id);

			_orderService.UpdateOrderAddress(cart, finalAddress);

			_productService.UpdateStockQuantities(cart.OrderItems);

			_orderService.UpdateOrderToState(cart, "Zpracovává se");

			

			return Json(new { success = true });
		}



	}

}
