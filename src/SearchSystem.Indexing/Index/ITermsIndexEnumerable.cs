using System.Collections.Generic;

// ReSharper disable BuiltInTypeReferenceStyle
using Term = System.String;
using DocLinks = System.Collections.Immutable.IImmutableSet<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.Indexing.Index
{
	/// <summary>
	/// Enumerable which represents indexed document links set.
	/// </summary>
	public interface ITermsIndexEnumerable : IEnumerable<KeyValuePair<Term, DocLinks>>
	{
	}
}