namespace SearchSystem.Normalization.Normalizer
{
	/// <summary>
	/// Word normalizer.
	/// </summary>
	internal interface INormalizer
	{
		/// <summary>
		/// Perform word normalization.
		/// </summary>
		string Normalize(string word);
	}
}