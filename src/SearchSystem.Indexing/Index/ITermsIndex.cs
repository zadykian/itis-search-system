// ReSharper disable BuiltInTypeReferenceStyle

using System.Collections.Generic;
using Term = System.String;
using DocLinks = System.Collections.Immutable.IImmutableSet<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.Indexing.Index
{
	/// <summary>
	/// Index of normalized documents.
	/// </summary>
	public interface ITermsIndex : IEnumerable<KeyValuePair<Term, DocLinks>>
	{
		/// <summary>
		/// Get set of document links which all contain term <paramref name="term"/>. 
		/// </summary>
		DocLinks AllWhichContains(Term term);

		/// <summary>
		/// Get set of all indexed document links.
		/// </summary>
		DocLinks AllDocuments();
	}
}