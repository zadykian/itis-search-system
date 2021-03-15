using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
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

		private DocumentsIndex(ConcurrentDictionary<Term, DocumentsSet> termsToDocuments)
			=> this.termsToDocuments = termsToDocuments;

		public DocumentsIndex(IEnumerable<IDocument> allDocuments)
			=> termsToDocuments = PerformIndexation(allDocuments);

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
		/// Represent itself as <see cref="IDocument"/> instance. 
		/// </summary>
		public IDocument AsDocument()
			=> JsonSerializer
				.Serialize(termsToDocuments)
				.To(serialized => new Document(string.Empty, "terms-index.json", new [] {serialized}));

		/// <summary>
		/// Convert <paramref name="indexDocument"/> to <see cref="IDocumentsIndex"/> instance. 
		/// </summary>
		public static IDocumentsIndex FromDocument(IDocument indexDocument)
			=> indexDocument
				.Lines
				.Single()
				.To(serialized => JsonSerializer.Deserialize<Dictionary<Term, IImmutableSet<DocumentLink>>>(serialized)!)
				.Select(pair => (
					Term: pair.Key,
					DocsSet: pair
						.Value
						.Cast<IDocumentLink>()
						.ToImmutableHashSet()))
				.Select(tuple => new KeyValuePair<Term, DocumentsSet>(tuple.Term, tuple.DocsSet))
				.To(keyValuePairs => new ConcurrentDictionary<Term, DocumentsSet>(keyValuePairs))
				.To(dictionary => new DocumentsIndex(dictionary));

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
				.To(keyValuePairs => new ConcurrentDictionary<Term, DocumentsSet>(keyValuePairs));
	}
}