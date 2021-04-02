using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Infrastructure.Words;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Indexing.Phase
{
	/// <inheritdoc cref="IIndexingEnginePhase"/>
	internal class IndexingEnginePhase : EnginePhaseBase<Docs, ITermsIndex>, IIndexingEnginePhase
	{
		private readonly IWordExtractor wordExtractor;

		public IndexingEnginePhase(
			IWordExtractor wordExtractor,
			IAppEnvironment<IndexingEnginePhase> appEnvironment) : base(appEnvironment)
			=> this.wordExtractor = wordExtractor;

		/// <inheritdoc />
		protected override async Task<ITermsIndex> ExecuteAnewAsync(Docs inputData)
		{
			var documentsIndex = new TermsIndex(inputData, wordExtractor);

			try
			{
				await documentsIndex
					.AsDocument(AppEnvironment.Storage.Conventions.TermsIndex)
					.To(AppEnvironment.Storage.SaveOrAppendAsync);
			}
			catch (Exception exception)
			{
				AppEnvironment.Logger.LogError(exception, "Failed to save created index to documents storage.");
			}

			return documentsIndex;
		}

		/// <inheritdoc />
		protected override async Task<ITermsIndex> LoadPreviousResultsAsync()
		{
			var document = await AppEnvironment.Storage.LoadAsync(AppEnvironment.Storage.Conventions.TermsIndex);
			return new TermsIndex(document);
		}
	}
}