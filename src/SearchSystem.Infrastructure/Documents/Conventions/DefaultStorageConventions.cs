namespace SearchSystem.Infrastructure.Documents.Conventions
{
	/// <inheritdoc />
	internal class DefaultStorageConventions : IStorageConventions
	{
		/// <inheritdoc />
		IDocumentLink IStorageConventions.WebPagesIndex => new DocumentLink(string.Empty, "web-pages-index.txt");

		/// <inheritdoc />
		IDocumentLink IStorageConventions.TermsIndex => new DocumentLink(string.Empty, "terms-index.json");

		/// <inheritdoc />
		IDocumentLink IStorageConventions.TermStats => new DocumentLink(string.Empty, "term-stats.txt");
	}
}