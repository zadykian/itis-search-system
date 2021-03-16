namespace SearchSystem.Infrastructure.Documents
{
	/// <inheritdoc cref="IDocumentLink"/>
	public record DocumentLink(string SubsectionName, string Name) : IDocumentLink;
}