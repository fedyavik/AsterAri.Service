namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Tone was detected on the channel.
	/// </summary>
	public class ChannelToneDetectedEvent : EventModel
	{
		/// <summary>
		/// The channel the tone was detected on.
		/// </summary>
		public ChannelModel Channel { get; set; }
	}
}
