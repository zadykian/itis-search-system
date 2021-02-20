namespace SearchSystem.WebCrawler
{
	/// <summary>
	/// Application parameters.
	/// </summary>
	internal class Parameters
	{
		/// <summary>
		/// Required words per page.
		/// </summary>
		public uint WordsPerPage => 1000;

		/// <summary>
		/// Total count of pages to be downloaded.
		/// </summary>
		public uint TotalPages => 100;
	}
}