using Microsoft.EntityFrameworkCore;
using PNET_semestralka.Data;
using PNET_semestralka.Models;

namespace PNET_semestralka.Services
{
	public class OrderService
	{
		private readonly ApplicationDbContext _context;

		public OrderService(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// returns a list of orders for a given customer
		/// </summary>
		/// <param name="customerId">the ID of the customer</param>
		/// <returns>a list of orders associated to the customer</returns>
		public List<Order> GetOrdersById(int customerId)
		{
			return _context.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.Where(o => o.CustomerId == customerId && o.Stav != "Košík")
				.ToList();
		}

		/// <summary>
		/// returns a list of all orders
		/// </summary>
		/// <returns>a list of all orders</returns>
		public List<Order> GetAllOrders()
		{
			return _context.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.Where(o => o.Stav != "Košík")
				.ToList();
		}

		/// <summary>
		/// returns a specific order by ID
		/// </summary>
		/// <param name="id">the ID of the order</param>
		/// <returns>the order with the specified ID or null if not found</returns>
		public Order? GetOrderById(int id)
		{
			return _context.Orders.FirstOrDefault(o => o.Id == id);
		}

		/// <summary>
		/// updates the state of an order
		/// </summary>
		/// <param name="order">the order to update</param>
		/// <param name="state">the new state of the order</param>
		public void UpdateOrderToState(Order order, string state)
		{
			order.Stav = state;
			_context.Orders.Update(order);
			_context.SaveChanges();
		}

		/// <summary>
		/// updates the address information for a given order
		/// </summary>
		/// <param name="order">the order to update</param>
		/// <param name="address">the new address to assign to the order</param>
		public void UpdateOrderAddress(Order order, SendingAddress address)
		{
			order.Jmeno = address.Jmeno;
			order.Prijmeni = address.Prijmeni;
			order.Ulice = address.Ulice;
			order.PopisneCislo = address.PopisneCislo;
			order.OrientacniCislo = address.OrientacniCislo;
			order.Psc = address.Psc;
			order.Mesto = address.Mesto;

			_context.SaveChanges();
		}
	}
}
