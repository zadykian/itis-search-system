using System;

// ReSharper disable BuiltInTypeReferenceStyle
using PageId = System.UInt32;

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
		PageId PageId { get; }

		/// <summary>
		/// Additional info which is displayed to user. 
		/// </summary>
		string AdditionalInfo();
	}
}