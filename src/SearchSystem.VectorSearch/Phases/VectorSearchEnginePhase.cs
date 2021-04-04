using System;
using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.SearchEnginePhases;

namespace SearchSystem.VectorSearch.Phases
{
	/// <inheritdoc cref="ISearchAlgorithmEnginePhase"/>
	internal class VectorSearchEnginePhase : EnginePhaseBase<ITermsIndex, Unit>, ISearchAlgorithmEnginePhase
	{
		public VectorSearchEnginePhase(IAppEnvironment<VectorSearchEnginePhase> appEnvironment) : base(appEnvironment)
		{
		}

		/// <inheritdoc />
		protected override Task<Unit> ExecuteAnewAsync(ITermsIndex termsIndex)
			=> throw new NotImplementedException();

		/// <inheritdoc />
		protected override Task<Unit> LoadPreviousResultsAsync()
			=> throw new NotImplementedException();
	}
}