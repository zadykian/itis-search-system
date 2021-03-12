using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.EnginePhases;

namespace SearchSystem.Lemmatization
{
	/// <inheritdoc />
	public interface ILemmatizationEnginePhase
		: ISearchEnginePhase<IReadOnlyCollection<IDocument>, IReadOnlyCollection<IDocument>>
	{
	}

	/// <inheritdoc cref="ILemmatizationEnginePhase"/>
	internal class LemmatizationEnginePhase :
		EnginePhaseBase<IReadOnlyCollection<IDocument>, IReadOnlyCollection<IDocument>>,
		ILemmatizationEnginePhase
	{
		public LemmatizationEnginePhase(IAppConfiguration appConfiguration, ILogger logger) : base(appConfiguration, logger)
		{
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<IDocument>> CreateNewData(IReadOnlyCollection<IDocument> inputData)
		{
			// todo
			await Task.CompletedTask;
			return ImmutableArray<IDocument>.Empty;
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<IDocument>> LoadPreviousResults()
		{
			// todo
			await Task.CompletedTask;
			return ImmutableArray<IDocument>.Empty;
		}
	}
}