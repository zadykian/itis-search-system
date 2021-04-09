using System;
using System.Threading.Tasks;

namespace SearchSystem.UserInteraction.UserInterface
{
	/// <inheritdoc />
	internal class ConsoleUserInterface : IUserInterface
	{
		/// <inheritdoc />
		void IUserInterface.ShowMessage(string message) => Console.WriteLine(message);

		/// <inheritdoc />
		Task<string> IUserInterface.ConsumeInputAsync() => Task.Run(() => Console.ReadLine() ?? string.Empty);
	}
}