using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents.Storage;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Indexing.Phase
{
	/// <inheritdoc cref="IIndexingEnginePhase"/>
	internal class IndexingEnginePhase : EnginePhaseBase<Docs, ITermsIndex>, IIndexingEnginePhase
	{
		private readonly IDocumentStorage documentStorage;

		public IndexingEnginePhase(
			IDocumentStorage documentStorage,
			IAppEnvironment<IndexingEnginePhase> appEnvironment) : base(appEnvironment)
			=> this.documentStorage = documentStorage;

		/// <inheritdoc />
		protected override async Task<ITermsIndex> ExecuteAnewAsync(Docs inputData)
		{
			var documentsIndex = new TermsIndex(inputData);

			try
			{
				await documentsIndex
					.AsDocument(documentStorage.Conventions.TermsIndex)
					.To(documentStorage.SaveOrAppendAsync);
			}
			catch (Exception exception)
			{
				Environment.Logger.LogError(exception, "Failed to save created index to documents storage.");
			}

			return documentsIndex;
		}

		/// <inheritdoc />
		protected override async Task<ITermsIndex> LoadPreviousResultsAsync()
		{
			var document = await documentStorage.LoadAsync(documentStorage.Conventions.TermsIndex);
			return new TermsIndex(document);
		}
	}
}