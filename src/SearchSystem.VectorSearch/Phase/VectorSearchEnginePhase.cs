using System.Threading.Tasks;
using SearchSystem.Indexing.Index;
using SearchSystem.Infrastructure.SearchEnginePhases;

namespace SearchSystem.VectorSearch.Phase
{
	/// <inheritdoc />
	internal class VectorSearchEnginePhase : IVectorSearchEnginePhase
	{
		/// <inheritdoc />
		public Task<Unit> ExecuteAsync(ITermsIndexEnumerable inputData) => throw new System.NotImplementedException();
	}
}