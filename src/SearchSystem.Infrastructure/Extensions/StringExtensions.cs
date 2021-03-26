using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace SearchSystem.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="string"/> type.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Split <paramref name="textLine"/> to words. 
		/// </summary>
		public static IEnumerable<string> Words(this string textLine)
			=> textLine
				.Split(' ', '\t', '\n')
				.Select(word => word
					.Where(character => char.IsLetterOrDigit(character) || character == '-')
					.Select(char.ToLower)
					.ToArray()
					.To(chars => new string(chars))
					.To(str => Regex.Replace(str, "(-)+", "-")))
				.Where(word => !string.IsNullOrWhiteSpace(word) && word != "-")
				.ToImmutableArray();
	}
}