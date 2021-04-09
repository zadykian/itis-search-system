using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Normalization.Normalizer;
using SearchSystem.UserInteraction.Process;

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
				.Select(entry => entry.Term)
				.Distinct()
				.OrderBy(term => term)
				.ToImmutableArray();

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
							.Select(term => wordsInDocument.TryGetValue(term, out var tfIdf) ? tfIdf : 0d)
							.ToImmutableArray();
					});

			await searchProcess.HandleSearchRequests(request =>
			{
				var requestWords = request
					.Split(' ')
					.Select(token => normalizer.Normalize(token))
					.ToImmutableArray();

			});
		}
	}
}