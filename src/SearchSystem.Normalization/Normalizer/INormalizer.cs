namespace SearchSystem.Normalization.Normalizer
{
	/// <summary>
	/// Word normalizer.
	/// </summary>
	public interface INormalizer
	{
		/// <summary>
		/// Perform word normalization.
		/// </summary>
		string Normalize(string word);
	}
}