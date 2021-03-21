using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Indexing.Phase;
using SearchSystem.Infrastructure.AppComponents;

[assembly: InternalsVisibleTo("SearchSystem.Tests")]

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