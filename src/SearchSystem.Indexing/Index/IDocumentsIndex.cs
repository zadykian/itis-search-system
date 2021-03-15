
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
		DocumentsSet AllWhichContains(Term term);
	}
}