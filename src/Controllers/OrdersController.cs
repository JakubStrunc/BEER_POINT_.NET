using Microsoft.AspNetCore.Mvc;
using PNET_semestralka.Controllers;
using PNET_semestralka.Data;
using PNET_semestralka.Migrations;
using PNET_semestralka.Data;
using PNET_semestralka.Migrations;
using PNET_semestralka.Services;

namespace PNET_semestralka.Controllers
{
	public class OrdersController : Controller
	{

		private readonly UserService _userService;
		private readonly OrderService _orderService;

		public OrdersController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_userService = new(context);
			_orderService = new(context);

		}
		public IActionResult Index()
		{

			var email = HttpContext.Session.GetString("Email");

			var user = _userService.GetUserByEmail(email);

			var orders = _orderService.GetOrdersById(user.Id);
			return View(orders);
		}
	}
}