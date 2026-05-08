namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Channel changed Connected Line.
	/// </summary>
	public class ChannelConnectedLineEvent : EventModel
	{
		/// <summary>
		/// The channel whose connected line has changed.
		/// </summary>
		public ChannelModel Channel { get; set; }
	}
}
