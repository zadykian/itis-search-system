using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using SearchSystem.BooleanSearch.Parsing;
using SearchSystem.BooleanSearch.Phase;
using SearchSystem.BooleanSearch.Scan;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppComponents;
using SearchSystem.Normalization.Normalizer;

[assembly: InternalsVisibleTo("SearchSystem.Tests")]

namespace SearchSystem.BooleanSearch
{
	/// <inheritdoc />
	public class BooleanSearchAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<ISearchExpressionParser, SearchExpressionParser>()
				.AddSingleton<IIndexScan>(provider =>
				{
					var underlyingIndexScan = new IndexScan();
					var normalizer = provider.GetRequiredService<INormalizer>();
					return new NormalizedIndexScan(underlyingIndexScan, normalizer);
				})
				.AddSingleton<ISearchAlgorithmEnginePhase, BooleanSearchEnginePhase>();
	}
}