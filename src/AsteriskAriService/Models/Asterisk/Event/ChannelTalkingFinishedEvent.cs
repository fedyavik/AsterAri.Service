namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Talking is no longer detected on the channel.
	/// </summary>
	public class ChannelTalkingFinishedEvent : EventModel
	{
		/// <summary>
		/// The channel on which talking completed.
		/// </summary>
		public ChannelModel Channel { get; set; }

		/// <summary>
		/// The length of time, in milliseconds, that talking was detected on the channel
		/// </summary>
		public int Duration { get; set; }
	}
}
