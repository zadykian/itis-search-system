using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.Infrastructure.Extensions;

namespace SearchSystem.Infrastructure.Documents.Storage
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementation stores documents as files in local directory.
	/// </remarks>
	internal class LocalFilesDocumentStorage : IDocumentStorage
	{
		/// <inheritdoc />
		async Task IDocumentStorage.SaveOrAppendAsync(IDocument document)
		{
			var subsectionDirectory = Path.Combine(CurrentDirectoryPath(), document.SubsectionName);
			Directory.CreateDirectory(subsectionDirectory);

			var currentFilePath = Path.Combine(subsectionDirectory, document.Name);
			await File.AppendAllLinesAsync(currentFilePath, document.Lines);
		}

		/// <inheritdoc />
		async Task<IDocument> IDocumentStorage.LoadAsync(IDocumentLink documentLink)
		{
			var documentLines = await Path
				.Combine(MostRecentDirectoryPath(), documentLink.RelativePath())
				.To(fileFullPath => File.ReadAllLinesAsync(fileFullPath));

			return new Document(documentLink.SubsectionName, documentLink.Name, documentLines);
		}

		/// <inheritdoc />
		IAsyncEnumerable<IDocument> IDocumentStorage.LoadFromSubsection(string subsectionName)
			=> MostRecentDirectoryPath()
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
		/// Get full path to most recently created subdirectory in <see cref="AppDataRootDirectoryPath"/> directory.
		/// </summary>
		private static string MostRecentDirectoryPath()
			=> AppDataRootDirectoryPath()
				.To(fullPath => new DirectoryInfo(fullPath))
				.GetDirectories()
				.OrderByDescending(directory => directory.CreationTime)
				.First()
				.FullName;

		/// <summary>
		/// Get full path to destination directory for current application session.
		/// </summary>
		private static string CurrentDirectoryPath()
			=> Process
				.GetCurrentProcess()
				.StartTime
				.ToString("yyyy-MM-dd_HH-mm-ss-fff")
				.To(currentDirectory => Path.Combine(AppDataRootDirectoryPath(), currentDirectory))
				.To(Directory.CreateDirectory)
				.FullName;

		/// <summary>
		/// Get full path to main results directory.
		/// </summary>
		private static string AppDataRootDirectoryPath() => Path.Combine(Environment.CurrentDirectory, "results");
	}
}