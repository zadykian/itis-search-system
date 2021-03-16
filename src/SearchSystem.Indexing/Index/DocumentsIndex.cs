using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Encodings.Web;
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
		private readonly IReadOnlyDictionary<Term, DocumentsSet> termsToDocuments;

		/// <param name="allDocuments">
		/// Documents to be indexed.
		/// </param>
		public DocumentsIndex(IEnumerable<IDocument> allDocuments)
			=> termsToDocuments = PerformIndexation(allDocuments);

		/// <param name="indexDocument">
		/// Document which represents serialized index.
		/// </param>
		public DocumentsIndex(IDocument indexDocument)
			=> termsToDocuments = FromDocument(indexDocument);

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
			=> new JsonSerializerOptions
				{
					WriteIndented = true,
					Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
				}
				.To(options => JsonSerializer.Serialize(termsToDocuments, options))
				.To(serialized => new Document(string.Empty, "terms-index.json", new [] {serialized}));

		/// <summary>
		/// Perform indexation of documents <paramref name="allDocuments"/>. 
		/// </summary>
		private static IReadOnlyDictionary<Term, DocumentsSet> PerformIndexation(IEnumerable<IDocument> allDocuments)
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
						.ToImmutableSortedSet()))
				.OrderBy(tuple => tuple.Term)
				.Select(tuple => new KeyValuePair<Term, DocumentsSet>(tuple.Term, tuple.DocsSet))
				.To(keyValuePairs => new Dictionary<Term, DocumentsSet>(keyValuePairs));

		/// <summary>
		/// Deserialize document <paramref name="indexDocument"/> to <see cref="termsToDocuments"/> dictionary.
		/// </summary>
		private static IReadOnlyDictionary<Term, DocumentsSet> FromDocument(IDocument indexDocument)
			=> indexDocument
				.Lines
				.Single()
				.To(serialized => JsonSerializer.Deserialize<Dictionary<Term, IImmutableSet<DocumentLink>>>(serialized)!)
				.Select(pair => (
					Term: pair.Key,
					DocsSet: pair
						.Value
						.Cast<IDocumentLink>()
						.ToImmutableSortedSet()))
				.OrderBy(tuple => tuple.Term)
				.Select(tuple => new KeyValuePair<Term, DocumentsSet>(tuple.Term, tuple.DocsSet))
				.To(keyValuePairs => new Dictionary<Term, DocumentsSet>(keyValuePairs));
	}
}