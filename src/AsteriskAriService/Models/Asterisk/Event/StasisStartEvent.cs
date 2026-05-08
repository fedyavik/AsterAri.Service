namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a channel has entered a Stasis application.
	/// </summary>
	public class StasisStartEvent : EventModel
	{
		/// <summary>
		/// Arguments to the application
		/// </summary>
		public List<string> Args { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ChannelModel Channel { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ChannelModel? Replace_channel { get; set; }
	}
}
