namespace PNET_semestralka.CommandDesignPatern
{
	/// <summary>
	/// defines the contract for command objects with an Execute method
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// executes the command
		/// </summary>
		void Execute();
	}
}
