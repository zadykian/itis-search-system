using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.WebCrawler.Pages;

namespace SearchSystem.WebCrawler
{
	/// <summary>
	/// File system representation.
	/// </summary>
	internal class FileSystem
	{
		private readonly Parameters parameters = new();

		/// <summary>
		/// Save web pages to file system. 
		/// </summary>
		public async Task SaveWebPagesAsync(IAsyncEnumerable<WebPage> webPages)
		{
			var destinationDirectory = GetDestinationDirectory();

			var indexFileStream = File.Open(
				Path.Combine(destinationDirectory, "index.txt"),
				FileMode.OpenOrCreate);

			await using var indexFileTextWriter = new StreamWriter(indexFileStream);

			await webPages
				.Zip(AsyncEnumerable.Range(start: 0, (int) parameters.TotalPages))
				.ForEachAwaitAsync(async tuple =>
				{
					var (webPage, index) = tuple;

					var currentUrl = $"{index}. {webPage.Uri}";
					await indexFileTextWriter.WriteLineAsync(currentUrl);
					await indexFileTextWriter.FlushAsync();
					Console.WriteLine($"'{currentUrl}' is saved.");

					var currentFilePath = Path.Combine(destinationDirectory, $"{index}.txt");
					await File.AppendAllLinesAsync(currentFilePath, webPage.AllVisibleLines());
				});

			Console.WriteLine("done.");
		}

		/// <summary>
		/// Get full path to destination directory. 
		/// </summary>
		private static string GetDestinationDirectory()
		{
			var destinationDirectoryPath = Path.Combine(
				Environment.CurrentDirectory,
				"pages",
				DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"));

			Directory.CreateDirectory(destinationDirectoryPath);
			return destinationDirectoryPath;
		}
	}
}