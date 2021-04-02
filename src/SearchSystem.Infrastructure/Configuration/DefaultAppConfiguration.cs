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
				.GetSection("Phases:Crawl:RootPageUri")
				.Value
				.To(stringValue => new Uri(stringValue));

		/// <inheritdoc />
		uint IAppConfiguration.WordsPerPage()
			=> configuration
				.GetSection("Phases:Crawl:WordsPerPage")
				.Value
				.To(uint.Parse);

		/// <inheritdoc />
		uint IAppConfiguration.TotalPages()
			=> configuration
				.GetSection("Phases:Crawl:TotalPages")
				.Value
				.To(uint.Parse);

		/// <inheritdoc />
		bool IAppConfiguration.UsePreviousResultsFor(string componentName)
			=> configuration
				.GetSection($"Phases:{componentName}:UsePreviousResults")
				.Value
				.To(str => !string.IsNullOrWhiteSpace(str) && bool.Parse(str));

		/// <inheritdoc />
		Language IAppConfiguration.DocumentsLanguage()
			=> configuration
				.GetSection("Phases:Normalization:DocumentsLanguage")
				.Value
				.To(stringValue => (Language) typeof(Language)
					.GetField(stringValue, BindingFlags.Public | BindingFlags.Static)!
					.GetValue(obj: null)!);
	}
}