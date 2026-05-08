namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a channel has been created.
	/// </summary>
	public class ChannelCreatedEvent : EventModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ChannelModel Channel { get; set; }

	}
}
