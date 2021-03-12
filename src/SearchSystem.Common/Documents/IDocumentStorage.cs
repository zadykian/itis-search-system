using System.Threading.Tasks;

namespace SearchSystem.Common.Documents
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
	}
}