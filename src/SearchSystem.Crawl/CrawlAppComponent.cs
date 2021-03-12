using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Crawl.Crawler;
using SearchSystem.Infrastructure.AppComponents;

namespace SearchSystem.Crawl
{
	/// <inheritdoc />
	public class CrawlAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<IWebCrawler, WebCrawler>()
				.AddSingleton<ICrawlEnginePhase, CrawlEnginePhase>();
	}
}