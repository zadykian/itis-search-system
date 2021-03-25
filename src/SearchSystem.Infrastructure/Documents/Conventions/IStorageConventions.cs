using SearchSystem.Infrastructure.Documents.Storage;

namespace SearchSystem.Infrastructure.Documents.Conventions
{
	/// <summary>
	/// Conventions for storing documents in <see cref="IDocumentStorage"/>.
	/// </summary>
	public interface IStorageConventions
	{
		/// <summary>
		/// Link to web pages index file.
		/// </summary>
		IDocumentLink WebPagesIndex { get; }

		/// <summary>
		/// Link to terms index file.
		/// </summary>
		IDocumentLink TermsIndex { get; }
	}
}