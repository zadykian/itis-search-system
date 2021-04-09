using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Accord.Math.Distances;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Normalization.Normalizer;
using SearchSystem.UserInteraction.Process;
using SearchSystem.UserInteraction.Result;

namespace SearchSystem.VectorSearch.Phases
{
	/// <inheritdoc cref="ISearchAlgorithmEnginePhase"/>
	internal class VectorSearchEnginePhase : TerminatingEnginePhaseBase<ITermsIndex>, ISearchAlgorithmEnginePhase
	{
		// todo: move vectorization to separate phase

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
				.ToImmutableSortedDictionary(entry => entry.Term, entry => entry.InverseDocFrequency);

			var vectors = termEntryStats
				.GroupBy(entry => entry.DocumentLink)
				.ToDictionary(
					group => group.Key,
					group =>
					{
						var wordsInDocument = group
							.DistinctBy(entry => entry.Term)
							.ToImmutableDictionary(entry => entry.Term, entry => entry.TfIdf);

						return allTermsInCorpus
							.Select(pair => wordsInDocument.TryGetValue(pair.Key, out var tfIdf) ? tfIdf : 0d)
							.ToImmutableArray();
					});

			await searchProcess.HandleSearchRequests(request =>
			{
				var requestTerms = request
					.Split(' ')
					.Select(token => normalizer.Normalize(token))
					.ToImmutableArray();

				var requestVector = allTermsInCorpus
					.Select(pair =>
					{
						var (currentTerm, inverseDocFrequency) = pair;
						var termFrequency = requestTerms.Count(term => term == currentTerm) / (double) requestTerms.Length;
						return termFrequency * inverseDocFrequency;
					})
					.ToImmutableArray();

				return vectors
					.Select(pair => (
						DocLink: pair.Key,
						Cosine: new Cosine().Similarity(requestVector.ToArray(), pair.Value.ToArray())))
					.Select(tuple => new WeightedResultItem(tuple.Cosine, tuple.DocLink))
					.OrderByDescending(resultItem => resultItem)
					.Take(10)
					.ToImmutableArray()
					.To(resultItems => new ISearchResult.Success(resultItems));
			});
		}
	}
}