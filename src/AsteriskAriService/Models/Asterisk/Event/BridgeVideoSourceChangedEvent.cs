namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that the source of video in a bridge has changed.
	/// </summary>
	public class BridgeVideoSourceChangedEvent : EventModel
	{
		/// <summary>
		/// no description provided
		/// </summary>
		public BridgeModel Bridge { get; set; }

		/// <summary>
		/// no description provided
		/// </summary>
		public string Old_video_source_id { get; set; }

	}
}
