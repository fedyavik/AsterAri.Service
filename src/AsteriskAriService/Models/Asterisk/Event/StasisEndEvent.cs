namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a channel has left a Stasis application.
	/// </summary>
	public class StasisEndEvent : EventModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ChannelModel Channel { get; set; }
	}
}
