using LemmaSharp;

namespace SearchSystem.Normalization.Normalizer
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementation performs normalization via lemmatization.
	/// </remarks>
	internal class Lemmatizer : INormalizer
	{
		/// <inheritdoc />
		string INormalizer.Normalize(string word)
		{
			var lemmatizer = new LemmatizerPrebuiltFull(LanguagePrebuilt.English);
			return lemmatizer.Lemmatize(word);
		}
	}
}