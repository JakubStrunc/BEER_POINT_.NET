using PNET_semestralka.Data;
using PNET_semestralka.DataDto;
using PNET_semestralka.Models;

namespace PNET_semestralka.Services
{
	public class AddressService
	{
		private readonly ApplicationDbContext _context;

		public AddressService(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// adds a new address to the database
		/// </summary>
		/// <param name="address">the address to add</param>
		public void AddAddress(SendingAddress address)
		{
			_context.SendingAddress.Add(address);
			_context.SaveChanges();
		}

		/// <summary>
		/// returns a list of addresses for a given customer
		/// </summary>
		/// <param name="customerId">the ID of the customer</param>
		/// <returns>a list of addresses associated with the customer</returns>
		public List<AddressDto> GetAddressesById(int customerId)
		{
			return _context.SendingAddress
				.Where(a => a.CustomerId == customerId)
				.Select(a => new AddressDto
				{
					Id = a.Id,
					FirstName = a.Jmeno,
					LastName = a.Prijmeni,
					Street = a.Ulice,
					HouseNumber = a.PopisneCislo,
					OrientationNumber = a.OrientacniCislo,
					PostalCode = a.Psc,
					City = a.Mesto
				})
				.ToList();
		}

		/// <summary>
		/// retrieves an address by its ID and associated user ID
		/// </summary>
		/// <param name="addressId">the ID of the address to retrieve</param>
		/// <param name="userId">the ID of the user associated with the address</param>
		/// <returns>the address that matches the specified address ID and user ID, or null if not found</returns>
		public SendingAddress GetAddressByIdAndUser(int? addressId, int userId)
		{
			return _context.SendingAddress
				.FirstOrDefault(a => a.Id == addressId && a.CustomerId == userId);
		}

		/// <summary>
		/// deletes a specified address from the database
		/// </summary>
		/// <param name="addressId">the ID of the address to delete</param>
		public void DeleteAddress(int addressId)
		{
			var address = _context.SendingAddress.Find(addressId);
			if (address != null)
			{
				_context.SendingAddress.Remove(address);
				_context.SaveChanges();
			}
		}

	}
}
