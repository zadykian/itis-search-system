using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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

			var destinationDirectory = Path.Combine(Environment.CurrentDirectory, "pages");
			Directory.CreateDirectory(destinationDirectory);

			var fileStream = File.Open(
				Path.Combine(destinationDirectory, "index.txt"),
				FileMode.OpenOrCreate);

			await using var textWriter = new StreamWriter(fileStream);

			await GetWebPages(rootUri)
				.Where(page => page.AllVisibleWords().Count >= Parameters.WordsPerPage)
				.Distinct()
				.Take((int) Parameters.TotalPages)
				.Zip(AsyncEnumerable.Range(start: 0, (int) Parameters.TotalPages))
				.ForEachAwaitAsync(async tuple =>
				{
					var (webPage, index) = tuple;
					var currentUrl = $"{index}. {webPage.Url}";
					await textWriter.WriteLineAsync(currentUrl);
					Console.WriteLine($"'{currentUrl}' is saved.");
					var currentFilePath = Path.Combine(destinationDirectory, $"{index}.txt");
					await File.AppendAllTextAsync(currentFilePath, webPage.RawContent());
				});
		}

		private static async IAsyncEnumerable<WebPage> GetWebPages(Uri pageUri)
		{
			var currentPage = await GetWebPage(pageUri);
			yield return currentPage;

			foreach (var childUrl in currentPage.ChildUrls())
			{
				yield return await GetWebPage(childUrl);

				await foreach (var childPage in GetWebPages(childUrl))
				{
					yield return childPage;
				}
			}
		}

		private static async Task<WebPage> GetWebPage(Uri pageUri)
		{
			var configuration = Configuration.Default.WithDefaultLoader();

			var document = await BrowsingContext
				.New(configuration)
				.OpenAsync(pageUri.ToString());

			return new CachedWebPage(document);
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
	}
}