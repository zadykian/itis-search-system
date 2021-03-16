using Microsoft.Extensions.DependencyInjection;
using SearchSystem.BooleanSearch.Phase;
using SearchSystem.Infrastructure.AppComponents;

namespace SearchSystem.BooleanSearch
{
	/// <inheritdoc />
	public class BooleanSearchAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<IBooleanSearchEnginePhase, BooleanSearchEnginePhase>();
	}
}