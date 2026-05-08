namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// A channel initiated a media hold.
	/// </summary>
	public class ChannelHoldEvent : EventModel
	{
		/// <summary>
		/// The channel that initiated the hold event.
		/// </summary>
		public ChannelModel Channel { get; set; }

		/// <summary>
		/// The music on hold class that the initiator requested.
		/// </summary>
		public string? Musicclass { get; set; }
	}
}
