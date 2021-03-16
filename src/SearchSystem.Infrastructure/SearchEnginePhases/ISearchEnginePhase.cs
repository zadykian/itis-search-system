using System.Threading.Tasks;

namespace SearchSystem.Infrastructure.SearchEnginePhases
{
	/// <summary>
	/// Representation of search engine single phase.
	/// </summary>
	/// <typeparam name="TIn">
	/// Type of input data required by this step.
	/// </typeparam>
	/// <typeparam name="TOut">
	/// Type of phase output.
	/// </typeparam>
	public interface ISearchEnginePhase<in TIn, TOut>
	{
		/// <summary>
		/// Execute phase. 
		/// </summary>
		Task<TOut> ExecuteAsync(TIn inputData);
	}
}