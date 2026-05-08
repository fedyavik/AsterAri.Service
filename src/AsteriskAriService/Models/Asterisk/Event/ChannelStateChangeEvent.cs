namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification of a channel's state change.
	/// </summary>
	public class ChannelStateChangeEvent : EventModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ChannelModel Channel { get; set; }
	}
}
