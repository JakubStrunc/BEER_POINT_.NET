using Microsoft.AspNetCore.Mvc;
using PNET_semestralka.CommandDesignPatern;
using PNET_semestralka.Data;
using PNET_semestralka.DataDto;
using PNET_semestralka.Migrations;
using PNET_semestralka.Models;
using PNET_semestralka.StrategyDesignPatern;

namespace PNET_semestralka.Controllers
{
    public class CartController : Controller
    {

        public MyDatabase _database;
        public CartController(ApplicationDbContext context) 
        {
            _database = new MyDatabase(context);
        }
        public IActionResult Index()
        {
			var email = HttpContext.Session.GetString("Email");

			var user = _database.GetUserByEmail(email);


			var cart = _database.GetCartForCustomer(user.Id);

            return View(cart);
        }

		[HttpPost]
		public IActionResult UpdateQuantity([FromBody] UpdateQuantityDto data)
		{
			var email = HttpContext.Session.GetString("Email");
			var user = _database.GetUserByEmail(email);

			if (user is not Customer customer)
				return Unauthorized();

			try
			{
				var command = new UpdateQuantityCommand(_database, customer.Id, data.ProductId, data.NewQuantity);
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
			var user = _database.GetUserByEmail(email);
			if (user is not Customer customer)
				return Unauthorized();

			try
			{
				var command = new RemoveProductCommand(_database, customer.Id, data.Id);
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

			var user = _database.GetUserByEmail(email);

			
			var addresses = _database.GetAddressesById(user.Id);

			
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
			var user = _database.GetUserByEmail(email);

			var cart = _database.GetCartForCustomer(user.Id);
			if (!_database.AreAllProductsInStock(cart))
				return Json(new { success = false, error = "Některé produkty už nejsou skladem." });


			IStrategy strategy;

			if (address.existingAddressId != null)
			{
				strategy = new ExistingAddressStrategy(address.existingAddressId, _database);
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

				strategy = new NewAddressStrategy(newAddress, _database);
			}

			var finalAddress = strategy.GetAddress(user.Id);

			_database.UpdateOrderAddress(cart, finalAddress);
			_database.UpdateOrderToState(cart, "Zpracovává se");

			_database.UpdateStockQuantities(cart.OrderItems);

			return Json(new { success = true });
		}



	}

}
