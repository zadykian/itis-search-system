using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Infrastructure.Words;

// ReSharper disable BuiltInTypeReferenceStyle
using DocName = System.String;
using TermStats = System.Collections.Generic.IReadOnlyCollection<SearchSystem.VectorSearch.TermEntryStats>;

namespace SearchSystem.VectorSearch.Phases
{
	/// <summary>
	/// Vector search subphase which is responsible
	/// for per-term statistics collecting.
	/// </summary>
	internal interface IStatsCollectionSubphase : ISearchEnginePhase<ITermsIndex, TermStats>
	{
	}

	/// <inheritdoc cref="IStatsCollectionSubphase"/>
	internal class StatsCollectionSubphase : EnginePhaseBase<ITermsIndex, TermStats>, IStatsCollectionSubphase
	{
		private readonly IWordExtractor wordExtractor;

		public StatsCollectionSubphase(
			IWordExtractor wordExtractor,
			IAppEnvironment<StatsCollectionSubphase> appEnvironment) : base(appEnvironment)
			=> this.wordExtractor = wordExtractor;

		/// <inheritdoc />
		protected override async Task<TermStats> ExecuteAnewAsync(ITermsIndex termsIndex)
		{
			var documents = await LoadAllDocumentsAsync(termsIndex);
			var termStatsEntries = CalculateTermEntryStats(termsIndex, documents);

			try
			{
				await termStatsEntries
					.Select(statsEntry => statsEntry.ToString())
					.To(AppEnvironment.Storage.Conventions.TermStats.ToDocument)
					.To(AppEnvironment.Storage.SaveOrAppendAsync);
			}
			catch (Exception exception)
			{
				AppEnvironment.Logger.LogError(exception, "Failed to save calculated stats to documents storage.");
			}

			return termStatsEntries;
		}

		/// <summary>
		/// Calculate statistic values for all terms in index <paramref name="termsIndex"/>. 
		/// </summary>
		private static IReadOnlyCollection<TermEntryStats> CalculateTermEntryStats(
			ITermsIndex termsIndex,
			IReadOnlyDictionary<DocName, DocWithWords> documents)
			=> termsIndex
				.SelectMany(pair =>
				{
					var (currentTerm, documentLinks) = pair;
					var inverseTermFrequency = Math.Log10(documents.Count / (double) documentLinks.Count);

					return documentLinks
						.Select(link => documents[link.Name])
						.Select(docWithWords =>
						{
							var (document, words) = docWithWords;
							var termFrequency = words.Count(term => term == currentTerm) / (double) words.Count;
							return new TermEntryStats(currentTerm, document, termFrequency, inverseTermFrequency);
						});
				})
				.OrderBy(stats => stats.Term)
				.ThenBy(stats => stats.DocumentLink)
				.ToImmutableArray();

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
		protected override async Task<TermStats> LoadPreviousResultsAsync()
			=> (await AppEnvironment.Storage.LoadAsync(AppEnvironment.Storage.Conventions.TermStats))
				.Lines
				.Select(TermEntryStats.ParseLine)
				.ToImmutableArray();

		/// <summary>
		/// Document with pre-parsed words list.
		/// </summary>
		private sealed record DocWithWords(IDocument Doc, IReadOnlyCollection<string> Words);
	}
}