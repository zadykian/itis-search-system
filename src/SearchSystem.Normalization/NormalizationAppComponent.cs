using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Infrastructure.AppComponents;

namespace SearchSystem.Normalization
{
	/// <inheritdoc />
	public class NormalizationAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection.AddSingleton<INormalizationEnginePhase, NormalizationEnginePhase>();
	}
}