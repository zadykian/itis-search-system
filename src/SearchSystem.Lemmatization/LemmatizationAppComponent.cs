using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Infrastructure.AppComponents;

namespace SearchSystem.Lemmatization
{
	/// <inheritdoc />
	public class LemmatizationAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection.AddSingleton<ILemmatizationEnginePhase, LemmatizationEnginePhase>();
	}
}