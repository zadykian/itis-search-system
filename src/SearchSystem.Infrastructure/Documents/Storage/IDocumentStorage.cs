using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchSystem.Infrastructure.Documents.Storage
{
	/// <summary>
	/// Document storage.
	/// </summary>
	public interface IDocumentStorage
	{
		/// <summary>
		/// Save document or append <see cref="IDocument.Lines"/> to existing one.
		/// </summary>
		Task SaveOrAppendAsync(IDocument document);

		/// <summary>
		/// Load documents from subsection named <paramref name="subsectionName"/>. 
		/// </summary>
		IAsyncEnumerable<IDocument> LoadFromSubsection(string subsectionName);
	}
}