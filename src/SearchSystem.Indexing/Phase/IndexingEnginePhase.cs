using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Documents.Storage;
using SearchSystem.Infrastructure.EnginePhases;

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
		protected override Task<IDocumentsIndex> CreateNewData(Docs inputData)
			=> throw new System.NotImplementedException();

		/// <inheritdoc />
		protected override async Task<IDocumentsIndex> LoadPreviousResults()
		{
			var document = await documentStorage.LoadAsync(new DocumentLink(string.Empty, "terms-index.json"));
			var documentIndex = JsonSerializer.Deserialize<DocumentsIndex>(document.Lines.Single())!;

			if (documentIndex is null)
			{
				const string message = "Failed to deserialize terms index from file.";
				Environment.Logger.LogError(message);
				throw new ApplicationException(message);
			}

			return documentIndex;
		}
	}
}