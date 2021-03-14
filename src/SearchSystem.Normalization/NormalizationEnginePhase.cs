using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Documents.Storage;
using SearchSystem.Infrastructure.EnginePhases;

namespace SearchSystem.Normalization
{
	/// <inheritdoc />
	public interface INormalizationEnginePhase
		: ISearchEnginePhase<IReadOnlyCollection<IDocument>, IReadOnlyCollection<IDocument>>
	{
	}

	/// <inheritdoc cref="INormalizationEnginePhase"/>
	internal class NormalizationEnginePhase :
		DocumentsOutputPhaseBase<IReadOnlyCollection<IDocument>>,
		INormalizationEnginePhase
	{
		public NormalizationEnginePhase(
			IDocumentStorage documentStorage,
			IAppConfiguration appConfiguration,
			ILogger<NormalizationEnginePhase> logger) : base(documentStorage, appConfiguration, logger)
		{
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<IDocument>> CreateNewData(IReadOnlyCollection<IDocument> inputData)
		{
			// todo
			await Task.CompletedTask;
			return ImmutableArray<IDocument>.Empty;
		}
	}
}