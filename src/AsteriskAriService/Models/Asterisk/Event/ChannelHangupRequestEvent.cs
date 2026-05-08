namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// A hangup was requested on the channel.
	/// </summary>
	public class ChannelHangupRequestEvent : EventModel
	{
		/// <summary>
		/// Integer representation of the cause of the hangup.
		/// </summary>
		public int Cause { get; set; }

		/// <summary>
		/// Integer representation of the technology-specific off-nominal cause of the hangup.
		/// </summary>
		public int? Tech_cause { get; set; }
		
		/// <summary>
		/// Whether the hangup request was a soft hangup request.
		/// </summary>
		public bool Soft { get; set; }

		/// <summary>
		/// The channel on which the hangup was requested.
		/// </summary>
		public ChannelModel Channel { get; set; }
	}
}
