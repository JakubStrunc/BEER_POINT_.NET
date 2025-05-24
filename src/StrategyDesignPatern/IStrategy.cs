using PNET_semestralka.Models;

namespace PNET_semestralka.StrategyDesignPatern
{

	/// <summary>
	/// defines the strategy interface for retrieving a user's address
	/// </summary>
	public interface IStrategy
	{
		/// <summary>
		/// retrieves the address associated with the specified user ID
		/// </summary>
		/// <param name="userId">the ID of the user whose address is being retrieved</param>
		/// <returns>the user's address</returns>
		SendingAddress GetAddress(int userId);
	}


}
