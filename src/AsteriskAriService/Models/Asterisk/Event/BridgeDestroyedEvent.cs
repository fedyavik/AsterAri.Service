namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a bridge has been destroyed.
	/// </summary>
	public class BridgeDestroyedEvent : EventModel
	{
		/// <summary>
		/// no description provided
		/// </summary>
		public BridgeModel Bridge { get; set; }
	}
}
