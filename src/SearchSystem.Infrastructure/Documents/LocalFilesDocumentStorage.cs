using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchSystem.Infrastructure.Extensions;

namespace SearchSystem.Infrastructure.Documents
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementation stores documents in local directory.
	/// </remarks>
	internal class LocalFilesDocumentStorage : IDocumentStorage
	{
		private static readonly string appDataRootDirectory = Path.Combine(Environment.CurrentDirectory, "results");
		private readonly string currentDirectoryPath = GetDestinationDirectory();
		private readonly ILogger<LocalFilesDocumentStorage> logger;

		public LocalFilesDocumentStorage(ILogger<LocalFilesDocumentStorage> logger) => this.logger = logger;

		/// <inheritdoc />
		async Task IDocumentStorage.SaveAsync(IDocument document)
		{
			var currentFilePath = Path.Combine(currentDirectoryPath, document.Name);
			await File.AppendAllLinesAsync(currentFilePath, document.Lines);
			logger.LogInformation($"document '{document.Name}' is saved.");
		}

		/// <inheritdoc />
		IAsyncEnumerable<IDocument> IDocumentStorage.LoadFromSubsection(string subsectionName)
			=> Directory
				.EnumerateDirectories(appDataRootDirectory)
				.OrderByDescending(directory => directory)
				.First()
				.To(mostResentDirectory => Path.Combine(mostResentDirectory, subsectionName))
				.To(Directory.EnumerateFiles)
				.ToAsyncEnumerable()
				.SelectAwait(async fileFullPath =>
				{
					var fileName = Path.GetFileName(fileFullPath);
					var fileLines = await File.ReadAllLinesAsync(fileFullPath);
					return new Document(subsectionName, fileName, fileLines);
				});

		/// <summary>
		/// Get full path to destination directory. 
		/// </summary>
		private static string GetDestinationDirectory()
		{
			var currentDirectoryName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
			var destinationDirectoryPath = Path.Combine(appDataRootDirectory, currentDirectoryName);
			Directory.CreateDirectory(destinationDirectoryPath);
			return destinationDirectoryPath;
		}
	}
}