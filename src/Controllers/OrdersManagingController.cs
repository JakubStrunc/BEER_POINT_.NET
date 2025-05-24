using Microsoft.AspNetCore.Mvc;
using PNET_semestralka.Data;
using PNET_semestralka.DataDto;
using PNET_semestralka.Migrations;
using PNET_semestralka.Services;

namespace PNET_semestralka.Controllers
{
    public class OrdersManagingController : Controller
    {
		private readonly OrderService _orderService;
		public OrdersManagingController(ApplicationDbContext context)
        {
			_orderService = new(context);
		}

        public IActionResult Index()
        {
            var orders = _orderService.GetAllOrders();
            return View(orders);
        }

		[HttpPost]
		public IActionResult UpdateOrderStatus([FromBody] UpdateOrderStatusDto data)
		{
			var order = _orderService.GetOrderById(data.Id);



			_orderService.UpdateOrderToState(order, data.Stav);

			return Json(new { success = true });
		}
	}

}

