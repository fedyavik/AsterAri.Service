namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Event showing the completion of a media playback operation.
	/// </summary>
	public class PlaybackFinishedEvent : EventModel
	{
		/// <summary>
		/// Playback control object
		/// </summary>
		public PlaybackModel Playback { get; set; }
	}
}
