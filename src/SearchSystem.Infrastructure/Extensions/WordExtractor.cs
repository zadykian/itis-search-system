using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using SearchSystem.Infrastructure.Configuration;

namespace SearchSystem.Infrastructure.Extensions
{
	/// <inheritdoc />
	internal class WordExtractor : IWordExtractor
	{
		private readonly IAppConfiguration appConfiguration;

		public WordExtractor(IAppConfiguration appConfiguration) => this.appConfiguration = appConfiguration;

		/// <inheritdoc />
		IEnumerable<string> IWordExtractor.Parse(string textLine)
			=> textLine
				.To(line => Regex.Split(line, @"[^\p{L}]*\p{Z}[^\p{L}]*"))
				.Select(word => Regex.Replace(word, "(-)+", "-"))
				.Select(word => word
					.Where(c => CharPredicate(c) || c == '-')
					.To(chars => new string(chars.ToArray())))
				.Select(word => word.Trim('-'))
				.Where(word => !string.IsNullOrWhiteSpace(word))
				.ToImmutableArray();

		/// <summary>
		/// Character predicate based on <see cref="IAppConfiguration.DocumentsLanguage"/> value.
		/// </summary>
		private Predicate<char> CharPredicate
			=> appConfiguration.DocumentsLanguage() switch
			{
				Language.English => c => char.ToLower(c).Between('a', 'z'),
				Language.Russian => c => char.ToLower(c).Between('а', 'я'),
				_ => throw new ArgumentOutOfRangeException(nameof(IAppConfiguration.DocumentsLanguage))
			};
	}
}