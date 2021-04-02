using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Infrastructure.Words;

namespace SearchSystem.VectorSearch.Phase
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
			var stats = await termsIndex
				.ToAsyncEnumerable()
				.SelectMany(pair =>
				{
					var (currentTerm, documentLinks) = pair;

					var inverseTermFrequency = Math.Log10(
						termsIndex.AllDocuments().Count / (double) documentLinks.Count);

					var res = documentLinks
						.ToAsyncEnumerable()
						.SelectAwait(async link => await AppEnvironment.Storage.LoadAsync(link))
						.Select(doc =>
						{
							var allTerms = doc.Lines.SelectMany(wordExtractor.Parse).ToImmutableArray();
							var termFrequency = allTerms.Count(term => term == currentTerm) / (double) allTerms.Length;
							return (
								Term:  currentTerm,
								Stats: new TermStatsEntry(doc, termFrequency, inverseTermFrequency));
						});

					return res;
				})
				.OrderBy(tuple => tuple.Term)
				.ThenBy(tuple => tuple.Stats.DocumentLink)
				.Select(tuple => $"{tuple.Term,24} {tuple.Stats.DocumentLink.Name,8} {tuple.Stats.TermFrequency,12} {tuple.Stats.InverseDocumentFrequency,12} {tuple.Stats.TfIdf,12}")
				.ToArrayAsync();

			var document = new Document(string.Empty, "term-stats.txt", stats);
			await AppEnvironment.Storage.SaveOrAppendAsync(document);

			return Unit.Instance;
		}

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResultsAsync()
			=> throw new NotImplementedException();
	}

	internal readonly struct TermStatsEntry
	{
		public TermStatsEntry(
			IDocumentLink documentLink,
			double termFrequency,
			double inverseDocumentFrequency)
		{
			DocumentLink = documentLink;
			TermFrequency = Round(termFrequency);
			InverseDocumentFrequency = Round(inverseDocumentFrequency);
		}

		public IDocumentLink DocumentLink { get; }

		/// <summary>
		/// 
		/// </summary>
		public double TermFrequency { get; }

		/// <summary>
		/// 
		/// </summary>
		public double InverseDocumentFrequency { get; }

		/// <summary>
		/// 
		/// </summary>
		public double TfIdf => Round(TermFrequency * InverseDocumentFrequency);

		/// <summary>
		/// Round <paramref name="coefficient"/> to suitable decimals count. 
		/// </summary>
		private static double Round(double coefficient)
			=> Math.Round(coefficient, 8, MidpointRounding.AwayFromZero);
	}
}