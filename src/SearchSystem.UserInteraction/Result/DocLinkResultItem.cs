using System.IO;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;

// ReSharper disable BuiltInTypeReferenceStyle
using PageId = System.UInt32;

namespace SearchSystem.UserInteraction.Result
{
	/// <summary>
	/// Search result item which contains only link to page's document.
	/// </summary>
	public class DocLinkResultItem : ISearchResultItem
	{
		private readonly IDocumentLink pageDocumentLink;

		/// <param name="pageDocumentLink">
		/// Link to document of page which this item corresponds to.
		/// </param>
		public DocLinkResultItem(IDocumentLink pageDocumentLink)
			=> this.pageDocumentLink = pageDocumentLink;

		/// <inheritdoc />
		public virtual int CompareTo(ISearchResultItem? other) => other?.PageId.CompareTo(PageId) ?? 1;

		/// <inheritdoc />
		public PageId PageId
			=> pageDocumentLink
				.Name
				.To(Path.GetFileNameWithoutExtension)!
				.To(PageId.Parse);

		/// <inheritdoc />
		public virtual string AdditionalInfo() => string.Empty;
	}
}