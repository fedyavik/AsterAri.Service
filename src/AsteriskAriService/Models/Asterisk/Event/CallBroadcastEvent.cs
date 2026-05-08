namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a channel is being broadcast to ARI applications for claiming.
	/// </summary>
	public class CallBroadcastEvent : EventModel	
	{
		/// <summary>
		/// The channel being broadcast.
		/// </summary>
		public ChannelModel Channel { get; set; }

		/// <summary>
		/// The caller ID number.
		/// </summary>
		public string? Caller { get; set; }
		
		/// <summary>
		/// The called number.
		/// </summary>
		public string? Called { get; set; }
	}
}
