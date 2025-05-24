using PNET_semestralka.Migrations;
using PNET_semestralka.Models;
using PNET_semestralka.Services;

namespace PNET_semestralka.StrategyDesignPatern
{

	/// <summary>
	/// strategy for adding a new address for the user
	/// </summary>
	public class NewAddressStrategy : IStrategy
	{
		private readonly SendingAddress _address;
		private readonly AddressService _addressService;

		public NewAddressStrategy(SendingAddress address, AddressService addressService)
		{
			_address = address;
			_addressService = addressService;
		}

		/// <summary>
		/// retrieves the address for the user. If the user already has 3 addresses, the oldest one is deleted
		/// </summary>
		/// <param name="userId">the ID of the user whose address is being added</param>
		/// <returns>the newly added address</returns>
		public SendingAddress GetAddress(int userId)
		{
			
			var existing = _addressService.GetAddressesById(userId);

			// delete the oldest address if there are already 3 addresses
			if (existing.Count >= 3)
			{
				var oldest = existing.OrderBy(a => a.Id).First();
				_addressService.DeleteAddress(oldest.Id);
			}

			_address.CustomerId = userId;
			_addressService.AddAddress(_address);
			return _address;
		}

	}



}
