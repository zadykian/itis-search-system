using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppComponents;
using SearchSystem.VectorSearch.Phases;

namespace SearchSystem.VectorSearch
{
	/// <inheritdoc />
	public class VectorSearchAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<IStatsCollectionSubphase, StatsCollectionSubphase>()
				.AddSingleton<ISearchAlgorithmEnginePhase, VectorSearchEnginePhase>();
	}
}