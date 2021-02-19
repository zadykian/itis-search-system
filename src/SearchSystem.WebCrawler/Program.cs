using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;

namespace SearchSystem.WebCrawler
{
	internal static class Program
	{
		private static async Task Main(string[] args)
		{
			if (!TryGetRootUri(args, out var rootUri))
			{
				return;
			}

			var webPage = await GetWebPage(rootUri);
			var words = webPage.AllVisibleWords();
		}

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

		private static async Task<WebPage> GetWebPage(Uri pageUri)
		{
			var configuration = Configuration.Default.WithDefaultLoader();

			var document = await BrowsingContext
				.New(configuration)
				.OpenAsync(pageUri.ToString());

			return new CachedWebPage(document);
		}
	}
}