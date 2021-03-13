using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.Infrastructure.Extensions;

namespace SearchSystem.Infrastructure.Documents.Storage
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementation stores documents in local directory.
	/// </remarks>
	internal class LocalFilesDocumentStorage : IDocumentStorage
	{
		private static string AppDataRootDirectory => Path.Combine(Environment.CurrentDirectory, "results");

		private static string CurrentDirectoryPath => GetDestinationDirectory();

		/// <inheritdoc />
		async Task IDocumentStorage.SaveOrAppendAsync(IDocument document)
		{
			var subsectionDirectory = Path.Combine(CurrentDirectoryPath, document.SubsectionName);
			Directory.CreateDirectory(subsectionDirectory);

			var currentFilePath = Path.Combine(subsectionDirectory, document.Name);
			await File.AppendAllLinesAsync(currentFilePath, document.Lines);
		}

		/// <inheritdoc />
		IAsyncEnumerable<IDocument> IDocumentStorage.LoadFromSubsection(string subsectionName)
			=> Directory
				.EnumerateDirectories(AppDataRootDirectory)
				.OrderByDescending(Path.GetFileName)
				.First()
				.To(mostResentDirectory => Path.Combine(mostResentDirectory, subsectionName))
				.To(Directory.EnumerateFiles)
				.OrderBy(Path.GetFileName)
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
			var destinationDirectoryPath = Path.Combine(AppDataRootDirectory, currentDirectoryName);
			Directory.CreateDirectory(destinationDirectoryPath);
			return destinationDirectoryPath;
		}
	}
}