using System.Linq;
using System.Threading.Tasks;
using SearchSystem.Infrastructure.AppEnvironment;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Infrastructure.SearchEnginePhases
{
	/// <inheritdoc />
	/// <remarks>
	/// This phase produces collection of documents as output.
	/// </remarks>
	public abstract class DocumentsOutputPhaseBase<TIn> : EnginePhaseBase<TIn, Docs>
	{
		protected DocumentsOutputPhaseBase(
			IAppEnvironment<DocumentsOutputPhaseBase<TIn>> appEnvironment) : base(appEnvironment)
		{
		}

		/// <inheritdoc />
		protected sealed override async Task<Docs> LoadPreviousResultsAsync()
			=> await AppEnvironment
				.Storage
				.LoadFromSubsection(ComponentName)
				.ToArrayAsync();
	}
}