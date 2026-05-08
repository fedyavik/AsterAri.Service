namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Dialing state has changed.
	/// </summary>
	public class DialEvent : EventModel
	{
		/// <summary>
		/// The calling channel.
		/// </summary>
		public ChannelModel? Caller { get; set; }

		/// <summary>
		/// The dialed channel.
		/// </summary>
		public ChannelModel Peer { get; set; }

		/// <summary>
		/// Forwarding target requested by the original dialed channel.
		/// </summary>
		public string? Forward { get; set; }

		/// <summary>
		/// Channel that the caller has been forwarded to.
		/// </summary>
		public ChannelModel? Forwarded { get; set; }

		/// <summary>
		/// The dial string for calling the peer channel.
		/// </summary>
		public string? Dialstring { get; set; }

		/// <summary>
		/// Current status of the dialing attempt to the peer.
		/// </summary>
		public string Dialstatus { get; set; }
	}
}
