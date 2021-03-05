using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.Common;

namespace SearchSystem.WebCrawler
{
	/// <summary>
	/// Application entry point.
	/// </summary>
	internal static class Program
	{
		/// <summary>
		/// Entry point method. 
		/// </summary>
		private static async Task Main(string[] args)
		{
			if (!TryGetRootUri(args, out var rootUri))
			{
				return;
			}

			var webPages = new Crawler().CrawlThrough(rootUri);
			await new FileSystem().SaveWebPagesAsync(webPages);
		}

		/// <summary>
		/// Try get root URL from arguments <paramref name="args"/>. 
		/// </summary>
		private static bool TryGetRootUri(
			string[] args,
			[NotNullWhen(returnValue: true)] out Uri? rootUri)
		{
			if (!args.Any())
			{
				Console.WriteLine("Expected root URI as first app argument.");
				rootUri = default;
				return false;
			}

			if (!Uri.TryCreate(args.First(), UriKind.Absolute, out rootUri))
			{
				Console.WriteLine("Invalid root URI.");
				return false;
			}

			return true;
		}
	}
}