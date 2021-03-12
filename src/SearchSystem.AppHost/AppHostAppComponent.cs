using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Infrastructure.AppComponents;

namespace SearchSystem.AppHost
{
	/// <inheritdoc />
	internal class AppHostAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection.AddHostedService<Worker>();
	}
}