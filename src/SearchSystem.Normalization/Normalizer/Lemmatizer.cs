using System;
using LemmaSharp;
using SearchSystem.Infrastructure.Configuration;
using SearchSystem.Infrastructure.Extensions;

namespace SearchSystem.Normalization.Normalizer
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementation performs normalization via lemmatization.
	/// </remarks>
	internal class Lemmatizer : INormalizer
	{
		private readonly ILemmatizer internalLemmatizer;

		public Lemmatizer(IAppConfiguration appConfiguration)
			=> internalLemmatizer = appConfiguration
				.DocumentsLanguage()
				.To(language => language switch
				{
					Language.English => LanguagePrebuilt.English,
					Language.Russian => LanguagePrebuilt.Russian,
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, message: null)
				})
				.To(languagePrebuilt => new LemmatizerPrebuiltFull(languagePrebuilt));

		/// <inheritdoc />
		string INormalizer.Normalize(string word)
			=> internalLemmatizer
				.Lemmatize(word)
				.ToLower();
	}
}