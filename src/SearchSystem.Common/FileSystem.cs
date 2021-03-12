using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SearchSystem.Common
{
	/// <summary>
	/// File system representation.
	/// </summary>
	public class FileSystem
	{
		private readonly string destinationDirectoryFullPath = GetDestinationDirectory();

		/// <summary>
		/// Save web pages to file system. 
		/// </summary>
		public async Task SaveWebPagesAsync(IAsyncEnumerable<IWebPage> webPages)
		{
			var indexFileStream = File.Open(
				Path.Combine(destinationDirectoryFullPath, "index.txt"),
				FileMode.OpenOrCreate);

			await using var indexFileTextWriter = new StreamWriter(indexFileStream);

			await webPages
				.Zip(AsyncEnumerable.Range(start: 0, int.MaxValue))
				.ForEachAwaitAsync(async tuple =>
				{
					var (webPage, pageIndex) = tuple;

					var currentUrl = $"{pageIndex}. {webPage.Uri}";
					await indexFileTextWriter.WriteLineAsync(currentUrl);
					await indexFileTextWriter.FlushAsync();
					Console.WriteLine($"'{currentUrl}' is saved.");

					var currentFilePath = Path.Combine(destinationDirectoryFullPath, "raw_text_files", $"{pageIndex}.txt");
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