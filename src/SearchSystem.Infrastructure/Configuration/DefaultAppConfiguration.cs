using System;
using Microsoft.Extensions.Configuration;
using SearchSystem.Infrastructure.Extensions;

namespace SearchSystem.Infrastructure.Configuration
{
	/// <inheritdoc />
	internal class DefaultAppConfiguration : IAppConfiguration
	{
		private readonly IConfiguration configuration;

		public DefaultAppConfiguration(IConfiguration configuration) => this.configuration = configuration;

		/// <inheritdoc />
		Uri IAppConfiguration.RootPageUri()
			=> configuration
				.GetSection("WebCrawler:RootPageUri")
				.Value
				.To(stringValue => new Uri(stringValue));

		/// <inheritdoc />
		uint IAppConfiguration.WordsPerPage()
			=> configuration
				.GetSection("WebCrawler:WordsPerPage")
				.Value
				.To(uint.Parse);

		/// <inheritdoc />
		uint IAppConfiguration.TotalPages()
			=> configuration
				.GetSection("WebCrawler:TotalPages")
				.Value
				.To(uint.Parse);

		/// <inheritdoc />
		bool IAppConfiguration.UsePreviousResultsFor(string componentName)
			=> configuration
				.GetSection($"{componentName}:UsePreviousResults")
				.Value
				.To(bool.Parse);
	}
}