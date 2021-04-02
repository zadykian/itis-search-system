using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Infrastructure.AppComponents;
using SearchSystem.VectorSearch.Phase;

namespace SearchSystem.VectorSearch
{
	/// <inheritdoc />
	public class VectorSearchAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<IVectorSearchEnginePhase, VectorSearchEnginePhase>();
	}
}