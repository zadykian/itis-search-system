using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Documents.Storage;

namespace SearchSystem.Infrastructure.EnginePhases
{
	/// <inheritdoc />
	/// <remarks>
	/// This phase produces collection of documents as output.
	/// </remarks>
	public abstract class DocumentsOutputPhaseBase<TIn> : EnginePhaseBase<TIn, IReadOnlyCollection<IDocument>>
	{
		protected DocumentsOutputPhaseBase(
			IDocumentStorage documentStorage,
			IAppConfiguration appConfiguration,
			ILogger<EnginePhaseBase<TIn, IReadOnlyCollection<IDocument>>> logger) : base(appConfiguration, logger)
			=> DocumentStorage = documentStorage;

		/// <inheritdoc cref="IDocumentStorage"/>
		protected IDocumentStorage DocumentStorage { get; }

		/// <inheritdoc />
		protected sealed override async Task<IReadOnlyCollection<IDocument>> LoadPreviousResults()
			=> await DocumentStorage
				.LoadFromSubsection(ComponentName)
				.ToArrayAsync();
	}
}