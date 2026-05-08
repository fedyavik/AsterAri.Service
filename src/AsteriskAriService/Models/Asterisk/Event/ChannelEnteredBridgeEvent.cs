namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a channel has entered a bridge.
	/// </summary>
	public class ChannelEnteredBridgeEvent : EventModel
	{
		/// <summary>
		/// 
		/// </summary>
		public BridgeModel Bridge { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ChannelModel Channel { get; set; }

	}
}
