using System;
using System.Reflection;
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
				.GetSection("Crawl:RootPageUri")
				.Value
				.To(stringValue => new Uri(stringValue));

		/// <inheritdoc />
		uint IAppConfiguration.WordsPerPage()
			=> configuration
				.GetSection("Crawl:WordsPerPage")
				.Value
				.To(uint.Parse);

		/// <inheritdoc />
		uint IAppConfiguration.TotalPages()
			=> configuration
				.GetSection("Crawl:TotalPages")
				.Value
				.To(uint.Parse);

		/// <inheritdoc />
		bool IAppConfiguration.UsePreviousResultsFor(string componentName)
			=> configuration
				.GetSection($"{componentName}:UsePreviousResults")
				.Value
				.To(bool.Parse);

		/// <inheritdoc />
		Language IAppConfiguration.DocumentsLanguage()
			=> configuration
				.GetSection("Normalization:DocumentsLanguage")
				.Value
				.To(stringValue => (Language) typeof(Language)
					.GetField(stringValue, BindingFlags.Public | BindingFlags.Static)!
					.GetValue(obj: null)!);
	}
}