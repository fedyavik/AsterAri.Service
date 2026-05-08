namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Event showing the start of a media playback operation.
	/// </summary>
	public class PlaybackStartedEvent : EventModel
	{
		/// <summary>
		/// Playback control object
		/// </summary>
		public PlaybackModel Playback { get; set; }
	}
}
