using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SearchSystem.BooleanSearch.Parsing;
using SearchSystem.BooleanSearch.Scan;
using SearchSystem.BooleanSearch.UserInterface;
using SearchSystem.Indexing.Index;
using SearchSystem.Indexing.Phase.External;
using SearchSystem.Infrastructure.AppEnvironment;
using SearchSystem.Infrastructure.Documents;
using SearchSystem.Infrastructure.Extensions;
using SearchSystem.Infrastructure.SearchEnginePhases;
using SearchSystem.Infrastructure.WebPages;
using Sprache;

// ReSharper disable BuiltInTypeReferenceStyle
using PageId = System.UInt32;
using DocLinks = System.Collections.Generic.IReadOnlyCollection<SearchSystem.Infrastructure.Documents.IDocumentLink>;

namespace SearchSystem.BooleanSearch.Phase
{
	/// <inheritdoc cref="ISearchAlgorithmEnginePhase" />
	internal class BooleanSearchEnginePhase : TerminatingEnginePhaseBase<ITermsIndex>, ISearchAlgorithmEnginePhase
	{
		private readonly IUserInterface userInterface;
		private readonly ISearchExpressionParser expressionParser;
		private readonly IIndexScan indexScan;

		public BooleanSearchEnginePhase(
			IUserInterface userInterface,
			ISearchExpressionParser expressionParser,
			IIndexScan indexScan,
			IAppEnvironment<BooleanSearchEnginePhase> appEnvironment) : base(appEnvironment)
		{
			this.userInterface = userInterface;
			this.expressionParser = expressionParser;
			this.indexScan = indexScan;
		}

		/// <inheritdoc />
		protected override async Task ExecuteAsync(ITermsIndex inputData)
		{
			userInterface.ShowMessage($"{Environment.NewLine}enter search expression:");
			var searchRequest = await userInterface.ConsumeInputAsync();

			var resultText = expressionParser.Parse(searchRequest) switch
			{
				IParseResult.Success success => await success
					.SearchExpression
					.To(expression =>
					{
						var stopwatch = Stopwatch.StartNew();
						var result = indexScan.Execute(inputData, expression);
						return (FoundDocs: result, stopwatch.Elapsed);
					})
					.To(tuple => StringRepresentation(tuple.FoundDocs, tuple.Elapsed)),

				IParseResult.Failure failure => failure.ErrorText,
				_ => throw new ArgumentOutOfRangeException(nameof(IParseResult))
			};

			userInterface.ShowMessage(resultText);
			userInterface.ShowMessage($"{Environment.NewLine}exit? [yes/no]");
			var input = await userInterface.ConsumeInputAsync();

			if (string.Equals(input, "yes", StringComparison.InvariantCultureIgnoreCase))
			{
				await ExecuteAsync(inputData);
			}
		}

		/// <summary>
		/// Get string representation of found docs list. 
		/// </summary>
		private async Task<string> StringRepresentation(
			IReadOnlyCollection<ISearchResultItem> searchResultItems,
			TimeSpan elapsed)
		{
			var webPagesDocument = await AppEnvironment.Storage.LoadAsync(AppEnvironment.Storage.Conventions.WebPagesIndex);

			return new WebPagesIndex(webPagesDocument)
				.SavedPages
				.Join(
					searchResultItems,
					entry => entry.PageId, item => item.PageId,
					(entry, item) => (Page: entry, Result: item))
				.OrderBy(tuple => tuple.Result)
				.Select(tuple => tuple
					.Result
					.AdditionalInfo()
					.To(info => string.IsNullOrWhiteSpace(info) ? string.Empty : $" ({info})")
					.To(info => $"{tuple.Page.PageId}. {tuple.Page.PageUri}{info}"))
				.BeginWith($@"Found {searchResultItems.Count} page(s). Elapsed {elapsed:s\.fff}s")
				.JoinBy(Environment.NewLine);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public interface ISearchResultItem : IComparable<ISearchResultItem>
	{
		/// <summary>
		/// Identifier of page which this item corresponds to.
		/// </summary>
		PageId PageId { get; }

		/// <summary>
		/// Additional info which is displayed to user. 
		/// </summary>
		string AdditionalInfo();
	}

	public class DocLinkResultItem : ISearchResultItem
	{
		private readonly IDocumentLink pageDocumentLink;

		/// <param name="pageDocumentLink">
		/// Link to document of page which this item corresponds to.
		/// </param>
		public DocLinkResultItem(IDocumentLink pageDocumentLink)
			=> this.pageDocumentLink = pageDocumentLink;

		/// <inheritdoc />
		public virtual int CompareTo(ISearchResultItem? other)
			=> other is null
				? 1
				: PageId.CompareTo(other.PageId);

		/// <inheritdoc />
		public PageId PageId
			=> pageDocumentLink
					.Name
					.To(Path.GetFileNameWithoutExtension)!
				.To(PageId.Parse);

		/// <inheritdoc />
		public virtual string AdditionalInfo() => string.Empty;
	}

	public class WeightedResultItem : DocLinkResultItem
	{
		private readonly double weight;

		/// <param name="weight">
		/// Weight of search item.
		/// </param>
		/// <param name="pageDocumentLink">
		/// Link to document of page which this item corresponds to.
		/// </param>
		public WeightedResultItem(
			double weight,
			IDocumentLink pageDocumentLink) : base(pageDocumentLink)
			=> this.weight = weight;

		/// <inheritdoc />
		public override int CompareTo(ISearchResultItem? other)
			=> other is WeightedResultItem weighted
				? weight.CompareTo(weighted.weight)
				: throw new ArgumentException("Invalid argument type.", nameof(other));

		/// <inheritdoc />
		public override string AdditionalInfo() => weight.ToString("F2", CultureInfo.InvariantCulture);
	}
}