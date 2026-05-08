namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a bridge has been created.
	/// </summary>
	public class BridgeCreatedEvent : EventModel
	{
		/// <summary>
		/// no description provided
		/// </summary>
		public BridgeModel Bridge { get; set; }

	}
}
