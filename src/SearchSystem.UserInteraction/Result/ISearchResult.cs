using System.Collections.Generic;

namespace SearchSystem.UserInteraction.Result
{
	public interface ISearchResult
	{
		/// <summary>
		/// Successful search result.
		/// </summary>
		record Success(IReadOnlyCollection<ISearchResultItem> Items) : ISearchResult;

		/// <summary>
		/// Unsuccessful search result.
		/// </summary>
		record Failure(string ErrorText) : ISearchResult;
	}
}