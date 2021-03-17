using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using SearchSystem.BooleanSearch.Parsing;
using SearchSystem.BooleanSearch.Phase;
using SearchSystem.Infrastructure.AppComponents;

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
				.AddSingleton<IBooleanSearchEnginePhase, BooleanSearchEnginePhase>();
	}
}