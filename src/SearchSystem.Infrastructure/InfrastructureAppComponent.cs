using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Infrastructure.AppComponents;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents.Conventions;
using SearchSystem.Infrastructure.Documents.Storage;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.Words;

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
				.AddSingleton<IStorageConventions, DefaultStorageConventions>()
				.AddSingleton<IDocumentStorage, LocalFilesDocumentStorage>()
				.AddSingleton<IWordExtractor, WordExtractor>();
	}
}