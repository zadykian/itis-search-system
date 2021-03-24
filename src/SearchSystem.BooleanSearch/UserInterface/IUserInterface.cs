using System.Threading.Tasks;

namespace SearchSystem.BooleanSearch.UserInterface
{
	/// <summary>
	/// Component with interaction with user.
	/// </summary>
	internal interface IUserInterface
	{
		/// <summary>
		/// Show text message to user.
		/// </summary>
		void ShowMessage(string message);

		/// <summary>
		/// Consume input from user.
		/// </summary>
		Task<string> ConsumeInputAsync();
	}
}