using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;

// ReSharper disable BuiltInTypeReferenceStyle
using Term = System.String;
using DocLinks = System.Collections.Immutable.IImmutableSet<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.Indexing.Index
{
	/// <inheritdoc />
	internal class TermsIndex : ITermsIndex
	{
		protected readonly IReadOnlyDictionary<Term, DocLinks> termsToDocuments;

		/// <param name="allDocuments">
		/// Documents to be indexed.
		/// </param>
		public TermsIndex(IEnumerable<IDocument> allDocuments)
			=> termsToDocuments = PerformIndexation(allDocuments);

		/// <param name="indexDocument">
		/// Document which represents serialized index.
		/// </param>
		public TermsIndex(IDocument indexDocument)
			=> termsToDocuments = FromDocument(indexDocument);

		/// <inheritdoc />
		DocLinks ITermsIndex.AllWhichContains(Term term)
			=> termsToDocuments.TryGetValue(term, out var documentsSet)
				? documentsSet
				: ImmutableHashSet<IDocumentLink>.Empty;

		/// <inheritdoc />
		DocLinks ITermsIndex.AllDocuments()
			=> termsToDocuments
				.Values
				.Aggregate(ImmutableHashSet<IDocumentLink>.Empty, (firstSet, secondSet) => firstSet.Union(secondSet));

		/// <summary>
		/// Represent itself as <see cref="IDocument"/> instance. 
		/// </summary>
		public IDocument AsDocument(IDocumentLink documentLink)
			=> JsonSerializer
				.Serialize(termsToDocuments, JsonOptions)
				.To(serialized => documentLink.ToDocument(serialized));

		/// <summary>
		/// Perform indexation of documents <paramref name="allDocuments"/>. 
		/// </summary>
		private static IReadOnlyDictionary<Term, DocLinks> PerformIndexation(IEnumerable<IDocument> allDocuments)
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
				.To(AsOrderedDictionary);

		/// <summary>
		/// Deserialize document <paramref name="indexDocument"/> to <see cref="termsToDocuments"/> dictionary.
		/// </summary>
		private static IReadOnlyDictionary<Term, DocLinks> FromDocument(IDocument indexDocument)
			=> indexDocument
				.Lines
				.JoinBy(Environment.NewLine)
				.To(serialized => JsonSerializer
					.Deserialize<Dictionary<Term, ImmutableSortedSet<IDocumentLink>>>(serialized, JsonOptions)!)
				.Select(pair => (
					Term: pair.Key,
					DocsSet: pair.Value))
				.To(AsOrderedDictionary);

		/// <summary>
		/// Convert sequence of tuples to ordered read-only dictionary. 
		/// </summary>
		private static IReadOnlyDictionary<Term, DocLinks> AsOrderedDictionary(
			IEnumerable<(Term Term, ImmutableSortedSet<IDocumentLink> DocsSet)> enumerable)
			=> enumerable
				.OrderBy(tuple => tuple.Term)
				.Select(tuple => new KeyValuePair<Term, DocLinks>(tuple.Term, tuple.DocsSet))
				.To(keyValuePairs => new Dictionary<Term, DocLinks>(keyValuePairs));

		/// <summary>
		/// Options for index serialization and deserialization.
		/// </summary>
		private static JsonSerializerOptions JsonOptions
			=> new()
			{
				WriteIndented = true,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
				Converters = { new LinkConverter() }
			};

		/// <summary>
		/// Json converter for <see cref="DocumentLink"/> type.
		/// </summary>
		private sealed class LinkConverter : JsonConverter<IDocumentLink>
		{
			/// <remarks>
			/// Index is always being created based on normalized documents.
			/// </remarks>
			private const string subsectionName = "Normalization";

			/// <inheritdoc />
			public override IDocumentLink Read(
				ref Utf8JsonReader reader,
				Type typeToConvert,
				JsonSerializerOptions options) => new DocumentLink(subsectionName, reader.GetString()!);

			/// <inheritdoc />
			public override void Write(
				Utf8JsonWriter writer,
				IDocumentLink value,
				JsonSerializerOptions options) => writer.WriteStringValue(value.Name);
		}
	}
}