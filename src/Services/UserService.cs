using Microsoft.AspNetCore.Identity;
using PNET_semestralka.Data;
using PNET_semestralka.Models;

namespace PNET_semestralka.Services
{
	public class UserService
	{
		private readonly ApplicationDbContext _context;

		public UserService(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// returns a user by their email address
		/// </summary>
		/// <param name="Email">the email address of the user</param>
		/// <returns>a user with the specified email or null if not found</returns>
		public User? GetUserByEmail(string Email)
		{
			return _context.Users.FirstOrDefault(u => u.Email == Email);
		}



		/// <summary>
		/// adds a new customer to the database
		/// </summary>
		/// <param name="username">the username of the customer.</param>
		/// <param name="email">the email address of the customer</param>
		/// <param name="password">the password for the customer account</param>
		public void AddCustomer(string username, string email, string password)
		{
			var customer = new Customer
			{
				UzivatelskeJmeno = username,
				Email = email
			};

			var hasher = new PasswordHasher<User>();
			customer.Heslo = hasher.HashPassword(customer, password);

			_context.Users.Add(customer); // EF recognizes it's a Customer
			_context.SaveChanges();
		}
	}
}
