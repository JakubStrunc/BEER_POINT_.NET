using Microsoft.AspNetCore.Mvc;
using PNET_semestralka.Controllers;
using PNET_semestralka.Data;
using PNET_semestralka.Migrations;
using PNET_semestralka.Data;
using PNET_semestralka.Migrations;

namespace PNET_semestralka.Controllers
{
	public class OrdersController : Controller
	{

		private readonly ILogger<HomeController> _logger;

		private readonly MyDatabase _database;

		public OrdersController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_database = new MyDatabase(context);

		}
		public IActionResult Index()
		{

			var email = HttpContext.Session.GetString("Email");

			var user = _database.GetUserByEmail(email);

			var orders = _database.GetOrdersByCustomerId(user.Id);
			return View(orders);
		}
	}
}