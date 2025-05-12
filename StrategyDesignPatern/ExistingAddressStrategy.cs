using PNET_semestralka.Migrations;
using PNET_semestralka.Models;

namespace PNET_semestralka.StrategyDesignPatern
{

	/// <summary>
	/// strategy for retrieving an existing address for the user
	/// </summary>
	public class ExistingAddressStrategy : IStrategy
	{
		private readonly int? _addressId;
		private readonly MyDatabase _database;

		public ExistingAddressStrategy(int? addressId, MyDatabase database)
		{
			_addressId = addressId;
			_database = database;
		}

		/// <summary>
		/// retrieves the existing address associated with provided user ID
		/// </summary>
		/// <param name="userId">the ID of the user whose address is being retrieved</param>
		/// <returns>the existing address or null if not found</returns>
		public SendingAddress GetAddress(int userId)
		{
			
			return _database.GetAddressByIdAndUser(_addressId, userId);
		}
	}
}
