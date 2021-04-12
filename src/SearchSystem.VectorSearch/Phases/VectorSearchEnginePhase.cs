using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Accord.Math.Distances;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Normalization.Normalizer;
using SearchSystem.UserInteraction.Process;
using SearchSystem.UserInteraction.Result;

// ReSharper disable BuiltInTypeReferenceStyle
using Term = System.String;
using Request = System.String;
using TfIdfVector = System.Collections.Generic.IReadOnlyCollection<System.Double>;

namespace SearchSystem.VectorSearch.Phases
{
	/// <inheritdoc cref="ISearchAlgorithmEnginePhase"/>
	internal class VectorSearchEnginePhase : TerminatingEnginePhaseBase<ITermsIndex>, ISearchAlgorithmEnginePhase
	{
		private readonly IStatsCollectionSubphase statsCollectionSubphase;
		private readonly ISearchProcess searchProcess;
		private readonly INormalizer normalizer;

		public VectorSearchEnginePhase(
			IStatsCollectionSubphase statsCollectionSubphase,
			ISearchProcess searchProcess,
			INormalizer normalizer,
			IAppEnvironment<VectorSearchEnginePhase> appEnvironment) : base(appEnvironment)
		{
			this.statsCollectionSubphase = statsCollectionSubphase;
			this.searchProcess = searchProcess;
			this.normalizer = normalizer;
		}

		/// <inheritdoc />
		protected override async Task ExecuteAsync(ITermsIndex inputData)
		{
			var termEntryStats = await statsCollectionSubphase.ExecuteAsync(inputData);

			var allTermsInCorpus = termEntryStats
				.DistinctBy(entry => entry.Term)
				.OrderBy(entry => entry.Term)
				.Select(entry => (entry.Term, entry.InverseDocFrequency))
				.ToImmutableArray();

			var vectors = CreateDocVectors(termEntryStats, allTermsInCorpus);

			await searchProcess.HandleSearchRequests(request =>
				string.IsNullOrWhiteSpace(request)
					? new ISearchResult.Failure("Input request cannot be empty.")
					: HandleValidRequest(request, allTermsInCorpus, vectors));
		}

		/// <summary>
		/// Perform vectorization for all documents.
		/// </summary>
		/// <param name="termEntryStats">
		/// Full list of per-term stats in corpus.
		/// </param>
		/// <param name="allTermsInCorpus">
		/// Unique list of terms with its' inverse document frequency.
		/// </param>
		private static IReadOnlyCollection<(IDocumentLink DocLink, TfIdfVector Vector)> CreateDocVectors(
			IEnumerable<TermEntryStats> termEntryStats,
			IReadOnlyCollection<(Term Term, double InverseDocFrequency)> allTermsInCorpus)
			=> termEntryStats
				.GroupBy(entry => entry.DocumentLink)
				.Select(group => (
					DocLink: group.Key,
					Vector: CreateSingleVector(allTermsInCorpus, group)))
				.ToImmutableArray();

		/// <summary>
		/// Create TF-IDF vector based on <paramref name="documentTermStats"/> from single document.
		/// </summary>
		private static TfIdfVector CreateSingleVector(
			IEnumerable<(Term Term, double InverseDocFrequency)> allTermsInCorpus,
			IEnumerable<TermEntryStats> documentTermStats)
		{
			var wordsInDocument = documentTermStats
				.DistinctBy(entry => entry.Term)
				.ToImmutableDictionary(entry => entry.Term, entry => entry.TfIdf);

			return allTermsInCorpus
				.Select(pair => wordsInDocument.TryGetValue(pair.Term, out var tfIdf) ? tfIdf : 0d)
				.ToImmutableArray();
		}

		/// <summary>
		/// Handle valid search request. 
		/// </summary>
		private ISearchResult.Success HandleValidRequest(
			Request request,
			IEnumerable<(Term Term, double InverseDocFrequency)> allTermsInCorpus,
			IEnumerable<(IDocumentLink Doc, TfIdfVector Vector)> vectors)
		{
			var requestTerms = request
				.Split(' ')
				.Select(token => normalizer.Normalize(token))
				.ToImmutableArray();

			var requestVector = allTermsInCorpus
				.Select(tuple =>
				{
					var (currentTerm, inverseDocFrequency) = tuple;
					var termFrequency = requestTerms.Count(term => term == currentTerm) / (double) requestTerms.Length;
					return termFrequency * inverseDocFrequency;
				})
				.ToImmutableArray();

			return vectors
				.Select(pair => (
					DocLink: pair.Doc,
					Cosine: new Cosine().Similarity(requestVector.ToArray(), pair.Vector.ToArray())))
				.Select(tuple => new WeightedResultItem(tuple.Cosine, tuple.DocLink))
				.OrderByDescending(resultItem => resultItem)
				.Take(10)
				.ToImmutableArray()
				.To(resultItems => new ISearchResult.Success(resultItems));
		}
	}
}