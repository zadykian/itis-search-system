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
			var documents = await termsIndex
				.AllDocuments()
				.ToAsyncEnumerable()
				.SelectAwait(async link => await AppEnvironment.Storage.LoadAsync(link))
				.ToDictionaryAsync(
					document => document.Name,
					document => (
						Doc: document,
						Words: document.Lines.SelectMany(wordExtractor.Parse).ToImmutableArray()));

			var stats = termsIndex
				.AsParallel()
				.WithDegreeOfParallelism(4)
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
							return (
								Term:  currentTerm,
								Stats: new TermStatsEntry(document, termFrequency, inverseTermFrequency));
						});
				})
				.Select(tuple =>
					$"{tuple.Term,24} {tuple.Stats.DocumentLink.Name,8} {tuple.Stats.TermFrequency,12:F8} {tuple.Stats.InverseDocumentFrequency,12:F8} {tuple.Stats.TfIdf,12:F8}")
				.ToArray();

			var resultDocument = new Document(string.Empty, "term-stats.txt", stats);
			await AppEnvironment.Storage.SaveOrAppendAsync(resultDocument);

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