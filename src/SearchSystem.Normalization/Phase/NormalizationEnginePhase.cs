using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Documents.Storage;
using SearchSystem.Infrastructure.EnginePhases;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Normalization.Phase
{
	/// <inheritdoc cref="INormalizationEnginePhase"/>
	internal class NormalizationEnginePhase : DocumentsOutputPhaseBase<Docs>, INormalizationEnginePhase
	{
		public NormalizationEnginePhase(
			IDocumentStorage documentStorage,
			IAppConfiguration appConfiguration,
			ILogger<NormalizationEnginePhase> logger) : base(documentStorage, appConfiguration, logger)
		{
		}

		/// <inheritdoc />
		protected override async Task<Docs> CreateNewData(Docs inputData)
		{
			// todo
			await Task.CompletedTask;
			return ImmutableArray<IDocument>.Empty;
		}
	}
}