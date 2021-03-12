namespace SearchSystem.Infrastructure.EnginePhases
{
	/// <summary>
	/// Type which represents empty input or output.
	/// </summary>
	public readonly struct Unit
	{
		/// <summary>
		/// Instance of unit type.
		/// </summary>
		public static Unit Instance => new();
	}
}