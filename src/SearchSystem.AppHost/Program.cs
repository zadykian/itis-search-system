using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SearchSystem.BooleanSearch;
using SearchSystem.Crawl;
using SearchSystem.Indexing;
using SearchSystem.Infrastructure;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Normalization;
using SearchSystem.VectorSearch;

namespace SearchSystem.AppHost
{
	/// <summary>
	/// Application entry point. 
	/// </summary>
	internal static class Program
	{
		/// <summary>
		/// Entry point method. 
		/// </summary>
		private static Task Main(string[] args)
			=> Host
				.CreateDefaultBuilder(args)
				.UseDefaultServiceProvider(options =>
				{
					options.ValidateScopes = true;
					options.ValidateOnBuild = true;
				})
				.ConfigureServices((_, services) => services
					.AddComponent<InfrastructureAppComponent>()
					.AddComponent<CrawlAppComponent>()
					.AddComponent<NormalizationAppComponent>()
					.AddComponent<IndexingAppComponent>()
					.To(AddSearchAlgorithmComponent)
					.AddComponent<AppHostAppComponent>())
				.Build()
				.RunAsync();

		/// <summary>
		/// Add suitable search algorithm component
		/// based on <see cref="IAppConfiguration.SearchMode"/> configuration parameter. 
		/// </summary>
		private static IServiceCollection AddSearchAlgorithmComponent(IServiceCollection serviceCollection)
			=> serviceCollection
				.BuildServiceProvider()
				.GetRequiredService<IAppConfiguration>()
				.SearchMode()
				.To(searchMode => searchMode switch
				{
					SearchMode.Boolean => serviceCollection.AddComponent<BooleanSearchAppComponent>(),
					SearchMode.Vector  => serviceCollection.AddComponent<VectorSearchAppComponent>(),
					_ => throw new ArgumentOutOfRangeException(nameof(searchMode))
				});
	}
}