using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using SearchSystem.Infrastructure.AppComponents;
using SearchSystem.Normalization.Normalizer;
using SearchSystem.Normalization.Phase;

[assembly: InternalsVisibleTo("SearchSystem.Tests")]

namespace SearchSystem.Normalization
{
	/// <inheritdoc />
	public class NormalizationAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<INormalizer, Lemmatizer>()
				.AddSingleton<INormalizationEnginePhase, NormalizationEnginePhase>();
	}
}