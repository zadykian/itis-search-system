using System;
using System.Threading.Tasks;
using SearchSystem.UserInteraction.Result;

// ReSharper disable BuiltInTypeReferenceStyle
using Request = System.String;

namespace SearchSystem.UserInteraction.Process
{
	/// <summary>
	/// Search process which is responsible for user interaction.
	/// </summary>
	public interface ISearchProcess
	{
		/// <summary>
		/// Handle user's search requests.
		/// </summary>
		/// <param name="searchCoreFunc">
		/// Function to handle requests.
		/// </param>
		Task HandleSearchRequests(Func<Request, ISearchResult> searchCoreFunc);
	}
}