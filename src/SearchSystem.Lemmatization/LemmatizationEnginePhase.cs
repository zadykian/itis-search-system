using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Documents.Storage;
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
		DocumentsOutputPhaseBase<IReadOnlyCollection<IDocument>>,
		ILemmatizationEnginePhase
	{
		public LemmatizationEnginePhase(
			IDocumentStorage documentStorage,
			IAppConfiguration appConfiguration,
			ILogger<LemmatizationEnginePhase> logger) : base(documentStorage, appConfiguration, logger)
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