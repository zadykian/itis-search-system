using System;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Dom;

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
		}

		private static bool TryGetRootUri(string[] args, out Uri rootUri)
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

			var urls = document
				.All
				.OfType<IHtmlAnchorElement>()
				.Select(element => element.Href)
				.Where(uriString => Uri.TryCreate(uriString, UriKind.Absolute, out _) && uriString != pageUri.ToString())
				.Select(uriString => new Uri(uriString));

			var content = document.ToHtml();
			return new WebPage(pageUri, content, urls);
		}
	}
}