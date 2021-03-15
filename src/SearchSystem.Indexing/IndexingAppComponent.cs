using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase;
using SearchSystem.Infrastructure.AppComponents;

namespace SearchSystem.Indexing
{
	/// <inheritdoc />
	public class IndexingAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<IIndexingEnginePhase, IndexingEnginePhase>();
	}
}