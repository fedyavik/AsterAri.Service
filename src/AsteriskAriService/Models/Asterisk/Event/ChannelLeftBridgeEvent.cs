namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a channel has left a bridge.
	/// </summary>
	public class ChannelLeftBridgeEvent : EventModel
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
