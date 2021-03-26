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
				.To(line => Regex.Split(line, @"[^\p{L}]*\p{Z}[^\p{L}]*"))
				.Where(word => !string.IsNullOrWhiteSpace(word))
				.ToImmutableArray();
	}
}