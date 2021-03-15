using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using SearchSystem.Infrastructure.Documents;

// ReSharper disable BuiltInTypeReferenceStyle
using Term = System.String;
using DocumentsSet = System.Collections.Immutable.IImmutableSet<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.Indexing.Index
{
	/// <inheritdoc />
	internal class DocumentsIndex : IDocumentsIndex
	{
		private readonly ConcurrentDictionary<Term, DocumentsSet> termsToDocuments;

		public DocumentsIndex(IEnumerable<IDocument> allDocuments) => termsToDocuments = PerformIndexation(allDocuments);

		/// <inheritdoc />
		DocumentsSet IDocumentsIndex.AllWhichContains(Term term)
			=> termsToDocuments.TryGetValue(term, out var documentsSet)
				? documentsSet
				: ImmutableHashSet<IDocumentLink>.Empty;

		/// <summary>
		/// Perform indexation of documents <paramref name="allDocuments"/>. 
		/// </summary>
		private static ConcurrentDictionary<Term, DocumentsSet> PerformIndexation(IEnumerable<IDocument> allDocuments)
		{
			throw new System.NotImplementedException();
		}
	}
}