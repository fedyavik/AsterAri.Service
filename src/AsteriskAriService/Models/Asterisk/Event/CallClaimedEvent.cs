namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a broadcast channel has been successfully claimed by an ARI application.
	/// </summary>
	public class CallClaimedEvent : EventModel	
	{
		/// <summary>
		/// The channel that was claimed.
		/// </summary>
		public ChannelModel Channel { get; set; }

		/// <summary>
		/// The name of the ARI application that claimed the channel.
		/// </summary>
		public string Winner_app { get; set; }
	}
}
