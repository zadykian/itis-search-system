using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using SearchSystem.BooleanSearch.Parsing;
using SearchSystem.BooleanSearch.Phase;
using SearchSystem.BooleanSearch.Scan;
using SearchSystem.BooleanSearch.UserInterface;
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
				.AddSingleton<IUserInterface, ConsoleUserInterface>()
				.AddSingleton<ISearchExpressionParser, SearchExpressionParser>()
				.AddSingleton<IIndexScan, IndexScan>()
				.AddSingleton<IBooleanSearchEnginePhase, BooleanSearchEnginePhase>();
	}
}