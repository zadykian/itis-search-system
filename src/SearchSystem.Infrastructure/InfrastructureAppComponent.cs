using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Infrastructure.AppComponents;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents.Storage;

[assembly: InternalsVisibleTo("SearchSystem.Tests")]

namespace SearchSystem.Infrastructure
{
	/// <inheritdoc />
	public class InfrastructureAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<IAppConfiguration, DefaultAppConfiguration>()
				.AddSingleton(typeof(IAppEnvironment<>), typeof(DefaultAppEnvironment<>))
				.AddSingleton<IDocumentStorage, LocalFilesDocumentStorage>();
	}
}