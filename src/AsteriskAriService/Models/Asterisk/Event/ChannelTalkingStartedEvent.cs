namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Talking was detected on the channel.
	/// </summary>
	public class ChannelTalkingStartedEvent : EventModel
	{
		/// <summary>
		/// The channel on which talking started.
		/// </summary>
		public ChannelModel Channel { get; set; }
	}
}
