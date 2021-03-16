using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SearchSystem.BooleanSearch;
using SearchSystem.Crawl;
using SearchSystem.Indexing;
using SearchSystem.Infrastructure;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Normalization;

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
					.AddComponent<BooleanSearchAppComponent>()
					.AddComponent<AppHostAppComponent>())
				.Build()
				.RunAsync();
	}
}