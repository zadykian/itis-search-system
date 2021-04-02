using System;
using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.SearchEnginePhases;

namespace SearchSystem.VectorSearch.Phase
{
	/// <inheritdoc cref="ISearchAlgorithmEnginePhase"/>
	internal class VectorSearchEnginePhase : EnginePhaseBase<ITermsIndex, Unit>, ISearchAlgorithmEnginePhase
	{
		public VectorSearchEnginePhase(
			IAppEnvironment<VectorSearchEnginePhase> appEnvironment) : base(appEnvironment)
		{
		}

		/// <inheritdoc />
		protected override Task<Unit> ExecuteAnewAsync(ITermsIndex inputData)
			=> throw new System.NotImplementedException();

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResultsAsync()
			=> throw new System.NotImplementedException();
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