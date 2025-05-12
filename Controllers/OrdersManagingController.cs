using Microsoft.AspNetCore.Mvc;
using PNET_semestralka.Data;
using PNET_semestralka.DataDto;
using PNET_semestralka.Migrations;

namespace PNET_semestralka.Controllers
{
    public class OrdersManagingController : Controller
    {
        private MyDatabase _database;
        public OrdersManagingController(ApplicationDbContext context)
        {
            _database = new MyDatabase(context);
        }

        public IActionResult Index()
        {
            var orders = _database.GetAllOrders();
            return View(orders);
        }

		[HttpPost]
		public IActionResult UpdateOrderStatus([FromBody] UpdateOrderStatusDto data)
		{
			var order = _database.GetOrderById(data.Id);


			
			_database.UpdateOrderToState(order, data.Stav);

			return Json(new { success = true });
		}
	}

}

