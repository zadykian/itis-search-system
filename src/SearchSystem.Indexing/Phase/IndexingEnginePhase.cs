using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Documents.Storage;
using SearchSystem.Infrastructure.EnginePhases;
using SearchSystem.Infrastructure.Extensions;

using Docs = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocument>;

namespace SearchSystem.Indexing.Phase
{
	/// <inheritdoc cref="IIndexingEnginePhase"/>
	internal class IndexingEnginePhase : EnginePhaseBase<Docs, IDocumentsIndex>, IIndexingEnginePhase
	{
		private readonly IDocumentStorage documentStorage;

		public IndexingEnginePhase(
			IDocumentStorage documentStorage,
			IAppEnvironment<IndexingEnginePhase> appEnvironment) : base(appEnvironment)
			=> this.documentStorage = documentStorage;

		/// <inheritdoc />
		protected override async Task<IDocumentsIndex> CreateNewData(Docs inputData)
		{
			var documentsIndex = new DocumentsIndex(inputData);

			try
			{
				await documentsIndex
					.AsDocument()
					.To(documentStorage.SaveOrAppendAsync);
			}
			catch (Exception exception)
			{
				Environment.Logger.LogError("Failed to save created index to documents storage.", exception);
			}

			return documentsIndex;
		}

		/// <inheritdoc />
		protected override async Task<IDocumentsIndex> LoadPreviousResults()
		{
			var document = await documentStorage.LoadAsync(new DocumentLink(string.Empty, "terms-index.json"));
			return new DocumentsIndex(document);
		}
	}
}