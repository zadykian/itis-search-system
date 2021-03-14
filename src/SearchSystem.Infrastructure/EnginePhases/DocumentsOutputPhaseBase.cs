using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Documents.Storage;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Infrastructure.EnginePhases
{
	/// <inheritdoc />
	/// <remarks>
	/// This phase produces collection of documents as output.
	/// </remarks>
	public abstract class DocumentsOutputPhaseBase<TIn> : EnginePhaseBase<TIn, Docs>
	{
		protected DocumentsOutputPhaseBase(
			IDocumentStorage documentStorage,
			IAppConfiguration appConfiguration,
			ILogger<EnginePhaseBase<TIn, Docs>> logger) : base(appConfiguration, logger)
			=> DocumentStorage = documentStorage;

		/// <inheritdoc cref="IDocumentStorage"/>
		protected IDocumentStorage DocumentStorage { get; }

		/// <inheritdoc />
		protected sealed override async Task<Docs> LoadPreviousResults()
			=> await DocumentStorage
				.LoadFromSubsection(ComponentName)
				.ToArrayAsync();
	}
}