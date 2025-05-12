using PNET_semestralka.Migrations;
using PNET_semestralka.Models;

namespace PNET_semestralka.StrategyDesignPatern
{

	/// <summary>
	/// strategy for adding a new address for the user
	/// </summary>
	public class NewAddressStrategy : IStrategy
	{
		private readonly SendingAddress _address;
		private readonly MyDatabase _database;

		public NewAddressStrategy(SendingAddress address, MyDatabase database)
		{
			_address = address;
			_database = database;
		}

		/// <summary>
		/// retrieves the address for the user. If the user already has 3 addresses, the oldest one is deleted
		/// </summary>
		/// <param name="userId">the ID of the user whose address is being added</param>
		/// <returns>the newly added address</returns>
		public SendingAddress GetAddress(int userId)
		{
			
			var existing = _database.GetAddressesById(userId);

			// delete the oldest address if there are already 3 addresses
			if (existing.Count >= 3)
			{
				var oldest = existing.OrderBy(a => a.Id).First();
				_database.DeleteAddress(oldest.Id);
			}

			_address.CustomerId = userId;
			_database.AddAddress(_address);
			return _address;
		}

	}



}
