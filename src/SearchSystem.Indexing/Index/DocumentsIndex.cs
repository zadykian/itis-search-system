using System.Collections.Concurrent;

// ReSharper disable BuiltInTypeReferenceStyle
using Term = System.String;
using DocumentsSet = System.Collections.Immutable.IImmutableSet<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.Indexing.Index
{
	/// <inheritdoc />
	internal class DocumentsIndex : IDocumentsIndex
	{
		private readonly ConcurrentDictionary<Term, DocumentsSet> termsToDocuments = new();
	}
}