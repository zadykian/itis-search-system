using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchSystem.Infrastructure.Documents
{
	/// <summary>
	/// Document storage.
	/// </summary>
	public interface IDocumentStorage
	{
		/// <summary>
		/// Save document. 
		/// </summary>
		Task SaveAsync(IDocument document);

		/// <summary>
		/// Load documents from subsection named <paramref name="subsectionName"/>. 
		/// </summary>
		IAsyncEnumerable<IDocument> LoadFromSubsection(string subsectionName);
	}
}