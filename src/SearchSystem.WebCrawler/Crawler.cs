using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SearchSystem.WebCrawler
{
	/// <summary>
	/// Web crawler.
	/// </summary>
	internal class Crawler
	{
		private readonly WebPages webPages = new();

		/// <summary>
		/// Save web pages to destination directory. 
		/// </summary>
		public async Task SaveWebPages(Uri rootUri)
		{
			var destinationDirectory = GetDestinationDirectory();

			var indexFileStream = File.Open(
				Path.Combine(destinationDirectory, "index.txt"),
				FileMode.OpenOrCreate);

			await using var textWriter = new StreamWriter(indexFileStream);

			await webPages
				.Download(rootUri)
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

		private static string GetDestinationDirectory()
		{
			var destinationDirectory = Path.Combine(Environment.CurrentDirectory, "pages");
			Directory.CreateDirectory(destinationDirectory);
			return destinationDirectory;
		}
	}
}