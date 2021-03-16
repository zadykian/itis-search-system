using System;

namespace SearchSystem.Infrastructure.Documents
{
	/// <inheritdoc cref="IDocumentLink"/>
	public record DocumentLink(string SubsectionName, string Name) : IDocumentLink, IComparable<DocumentLink>, IComparable
	{
		/// <inheritdoc />
		public int CompareTo(DocumentLink? other)
		{
			if (ReferenceEquals(this, other)) return 0;
			if (ReferenceEquals(null, other)) return 1;
			var subsectionNameComparison = string.Compare(SubsectionName, other.SubsectionName, StringComparison.OrdinalIgnoreCase);

			return subsectionNameComparison != 0
				? subsectionNameComparison
				: string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
		}

		/// <inheritdoc />
		public int CompareTo(object? obj)
		{
			if (ReferenceEquals(null, obj)) return 1;
			if (ReferenceEquals(this, obj)) return 0;
			return obj is DocumentLink other
				? CompareTo(other)
				: throw new ArgumentException($"Object must be of type {nameof(DocumentLink)}");
		}
	}
}