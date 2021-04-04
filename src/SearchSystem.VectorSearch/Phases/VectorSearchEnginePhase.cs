// ReSharper disable BuiltInTypeReferenceStyle
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Infrastructure.Words;
using Term = System.String;
using DocName = System.String;

namespace SearchSystem.VectorSearch.Phases
{
	/// <inheritdoc cref="ISearchAlgorithmEnginePhase"/>
	internal class VectorSearchEnginePhase : EnginePhaseBase<ITermsIndex, Unit>, ISearchAlgorithmEnginePhase
	{
		private readonly IWordExtractor wordExtractor;

		public VectorSearchEnginePhase(
			IWordExtractor wordExtractor,
			IAppEnvironment<VectorSearchEnginePhase> appEnvironment) : base(appEnvironment)
			=> this.wordExtractor = wordExtractor;

		/// <inheritdoc />
		protected override async Task<Unit> ExecuteAnewAsync(ITermsIndex termsIndex)
		{
			var documents = await LoadAllDocumentsAsync(termsIndex);

			var termStatsEntries = termsIndex
				.SelectMany(pair =>
				{
					var (currentTerm, documentLinks) = pair;
					var inverseTermFrequency = Math.Log10(documents.Count / (double) documentLinks.Count);

					return documentLinks
						.Select(link => documents[link.Name])
						.Select(tuple =>
						{
							var (document, words) = tuple;
							var termFrequency = words.Count(term => term == currentTerm) / (double) words.Length;
							return new TermEntryStats(currentTerm, document, termFrequency, inverseTermFrequency);
						});
				})
				.OrderBy(stats => stats.Term)
				.ThenBy(stats => stats.DocumentLink)
				.Select(tuple => tuple.ToString())
				.ToImmutableArray();

			var termStatsDocument = AppEnvironment.Storage.Conventions.TermStats.ToDocument(termStatsEntries);
			await AppEnvironment.Storage.SaveOrAppendAsync(termStatsDocument);
			return Unit.Instance;
		}

		/// <summary>
		/// Load all documents which are present in <paramref name="termsIndex"/> from documents storage.  
		/// </summary>
		private async Task<IReadOnlyDictionary<DocName, DocWithWords>> LoadAllDocumentsAsync(ITermsIndex termsIndex)
			=> await termsIndex
				.AllDocuments()
				.ToAsyncEnumerable()
				.SelectAwait(async link => await AppEnvironment.Storage.LoadAsync(link))
				.ToDictionaryAsync(
					document => document.Name,
					document => new DocWithWords(
						document,
						document.Lines.SelectMany(wordExtractor.Parse).ToImmutableArray()));

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResultsAsync()
			=> throw new NotImplementedException();

		/// <summary>
		/// Document with pre-parsed words list.
		/// </summary>
		private sealed record DocWithWords(IDocument Doc, ImmutableArray<string> Words);

		/// <summary>
		/// Struct which contains statistics for term <see cref="TermEntryStats.Term"/>
		/// in context of document <see cref="TermEntryStats.DocumentLink"/>.
		/// </summary>
		private readonly struct TermEntryStats
		{
			public TermEntryStats(
				Term term,
				IDocumentLink documentLink,
				double termFrequency,
				double inverseDocumentFrequency)
			{
				Term = term;
				DocumentLink = documentLink;
				TermFrequency = termFrequency;
				InverseDocumentFrequency = inverseDocumentFrequency;
			}

			/// <summary>
			/// Term.
			/// </summary>
			public Term Term { get; }

			/// <summary>
			/// Link to document.
			/// </summary>
			public IDocumentLink DocumentLink { get; }

			/// <summary>
			/// <para>
			/// Term's frequency.
			/// </para>
			/// <para>
			/// This value represents value ratio between count of entries <see cref="Term"/>
			/// and total words count in document <see cref="DocumentLink"/>.
			/// </para>
			/// </summary>
			private double TermFrequency { get; }

			/// <summary>
			/// <para>
			/// Inverse document frequency.
			/// </para>
			/// <para>
			/// This value represents logarithm of ratio between total documents count and count
			/// of documents which contains term <see cref="Term"/>.
			/// </para>
			/// </summary>
			private double InverseDocumentFrequency { get; }

			/// <summary>
			/// <para>
			/// Term frequency — Inverse document frequency.
			/// </para>
			/// <para>
			/// This value represents product of <see cref="TermFrequency"/> and <see cref="InverseDocumentFrequency"/>.
			/// </para>
			/// </summary>
			private double TfIdf => TermFrequency * InverseDocumentFrequency;

			/// <inheritdoc />
			public override string ToString()
				=> $"{Term,32} {DocumentLink.Name,8} {TermFrequency,12:F8} {InverseDocumentFrequency,12:F8} {TfIdf,12:F8}";
		}
	}
}