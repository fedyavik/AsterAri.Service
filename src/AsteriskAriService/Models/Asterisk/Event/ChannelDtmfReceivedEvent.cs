namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// DTMF received on a channel.
	/// This event is sent when the DTMF ends.
	/// There is no notification about the start of DTMF
	/// </summary>
	public class ChannelDtmfReceivedEvent : EventModel
	{
		/// <summary>
		/// DTMF digit received (0-9, A-E, # or *)
		/// </summary>
		public char Digit { get; set; }

		/// <summary>
		/// Number of milliseconds DTMF was received
		/// </summary>
		public int Duration_ms { get; set; }

		/// <summary>
		/// The channel on which DTMF was received
		/// </summary>
		public ChannelModel Channel { get; set; }

	}
}
