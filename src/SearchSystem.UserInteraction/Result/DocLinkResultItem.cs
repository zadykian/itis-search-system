using System;
using System.IO;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;

namespace SearchSystem.UserInteraction.Result
{
	public class DocLinkResultItem : ISearchResultItem
	{
		private readonly IDocumentLink pageDocumentLink;

		/// <param name="pageDocumentLink">
		/// Link to document of page which this item corresponds to.
		/// </param>
		public DocLinkResultItem(IDocumentLink pageDocumentLink)
			=> this.pageDocumentLink = pageDocumentLink;

		/// <inheritdoc />
		public virtual int CompareTo(ISearchResultItem? other)
			=> other is null
				? 1
				: PageId.CompareTo(other.PageId);

		/// <inheritdoc />
		public UInt32 PageId
			=> pageDocumentLink
					.Name
					.To(Path.GetFileNameWithoutExtension)!
				.To(UInt32.Parse);

		/// <inheritdoc />
		public virtual string AdditionalInfo() => string.Empty;
	}
}