using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;

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

		/// <inheritdoc />
		DocumentsSet IDocumentsIndex.AllDocuments()
			=> termsToDocuments
				.Values
				.Aggregate(ImmutableHashSet<IDocumentLink>.Empty, (firstSet, secondSet) => firstSet.Union(secondSet));

		/// <summary>
		/// Perform indexation of documents <paramref name="allDocuments"/>. 
		/// </summary>
		private static ConcurrentDictionary<Term, DocumentsSet> PerformIndexation(IEnumerable<IDocument> allDocuments)
			=> allDocuments
				.SelectMany(document => document
					.Lines
					.SelectMany(line => line.Words())
					.Select(term => (Term: term, Document: document)))
				.GroupBy(tuple => tuple.Term)
				.Select(group => (
					Term: group.Key,
					DocsSet: group
						.Select(tuple => new DocumentLink(tuple.Document.SubsectionName, tuple.Document.Name))
						.Cast<IDocumentLink>()
						.ToImmutableHashSet()))
				.Select(tuple => new KeyValuePair<Term, DocumentsSet>(tuple.Term, tuple.DocsSet))
				.To(keyValuePairs => new ConcurrentDictionary<string, DocumentsSet>(keyValuePairs));
	}
}