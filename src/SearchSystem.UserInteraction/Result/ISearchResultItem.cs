using System;

namespace SearchSystem.UserInteraction.Result
{
	/// <summary>
	/// Single item in <see cref="ISearchResult.Success"/>.
	/// </summary>
	public interface ISearchResultItem : IComparable<ISearchResultItem>
	{
		/// <summary>
		/// Identifier of page which this item corresponds to.
		/// </summary>
		UInt32 PageId { get; }

		/// <summary>
		/// Additional info which is displayed to user. 
		/// </summary>
		string AdditionalInfo();
	}
}