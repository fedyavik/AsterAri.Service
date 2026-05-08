namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that one bridge has merged into another.
	/// </summary>
	public class BridgeMergedEvent : EventModel
	{
		/// <summary>
		/// no description provided
		/// </summary>
		public BridgeModel Bridge { get; set; }

		/// <summary>
		/// no description provided
		/// </summary>
		public BridgeModel Bridge_from { get; set; }
	}
}
