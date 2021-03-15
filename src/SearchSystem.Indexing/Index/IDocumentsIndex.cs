// ReSharper disable BuiltInTypeReferenceStyle

using Term = System.String;
using DocumentsSet = System.Collections.Immutable.IImmutableSet<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.Indexing.Index
{
	/// <summary>
	/// Index of normalized documents.
	/// </summary>
	public interface IDocumentsIndex
	{
		/// <summary>
		/// Get set of document links which all contain term <paramref name="term"/>. 
		/// </summary>
		DocumentsSet AllWhichContains(Term term);

		/// <summary>
		/// Get set of all indexed document links.
		/// </summary>
		DocumentsSet AllDocuments();
	}
}