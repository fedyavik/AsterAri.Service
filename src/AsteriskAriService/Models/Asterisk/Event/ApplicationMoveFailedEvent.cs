namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that trying to move a channel to another Stasis application failed.
	/// </summary>
	public class ApplicationMoveFailedEvent : EventModel
	{
		/// <summary>
		/// no description provided
		/// </summary>
		public ChannelModel Channel { get; set; }

		/// <summary>
		/// no description provided
		/// </summary>
		public string Destination { get; set; }

		/// <summary>
		/// Arguments to the application
		/// </summary>
		public List<string> Args { get; set; }
	}
}
