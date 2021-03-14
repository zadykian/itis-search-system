using System;

namespace SearchSystem.Infrastructure.Configuration
{
	/// <summary>
	/// Application configuration.
	/// </summary>
	public interface IAppConfiguration
	{
		/// <summary>
		/// Uri of page which recursive search starts from.
		/// </summary>
		Uri RootPageUri();

		/// <summary>
		/// Required minimum words per page.
		/// </summary>
		uint WordsPerPage();

		/// <summary>
		/// Total count of pages being saved.
		/// </summary>
		uint TotalPages();

		/// <summary>
		/// Do not perform new calculations in context of component named <paramref name="componentName"/>. 
		/// </summary>
		bool UsePreviousResultsFor(string componentName);

		/// <summary>
		/// Language of documents being normalized.
		/// </summary>
		Language DocumentsLanguage();
	}
}