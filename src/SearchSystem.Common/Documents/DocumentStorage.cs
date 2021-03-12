using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SearchSystem.Common.Documents
{
	/// <inheritdoc />
	internal class DocumentStorage : IDocumentStorage
	{
		private readonly string currentDirectoryPath = GetDestinationDirectory();
		private readonly ILogger<DocumentStorage> logger;

		public DocumentStorage(ILogger<DocumentStorage> logger) => this.logger = logger;

		/// <inheritdoc />
		async Task IDocumentStorage.SaveAsync(IDocument document)
		{
			var currentFilePath = Path.Combine(currentDirectoryPath, document.RelativePath);
			await File.AppendAllLinesAsync(currentFilePath, document.Lines());
			logger.LogInformation($"document '{document.RelativePath}' is saved.");
		}

		/// <summary>
		/// Get full path to destination directory. 
		/// </summary>
		private static string GetDestinationDirectory()
		{
			var destinationDirectoryPath = Path.Combine(
				Environment.CurrentDirectory,
				"results",
				DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"));

			Directory.CreateDirectory(destinationDirectoryPath);
			return destinationDirectoryPath;
		}
	}
}