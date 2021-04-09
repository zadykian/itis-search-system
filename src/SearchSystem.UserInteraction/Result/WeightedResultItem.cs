using System;
using System.Globalization;
using SearchSystem.Infrastructure.Documents;

namespace SearchSystem.UserInteraction.Result
{
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