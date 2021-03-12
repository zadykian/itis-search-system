using System.Collections.Generic;

namespace SearchSystem.Common.Documents
{
	/// <inheritdoc cref="IDocument"/>
	public record Document(string RelativePath, IReadOnlyCollection<string> Lines) : IDocument;
}