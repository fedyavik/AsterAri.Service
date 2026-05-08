namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Event showing the continuation of a media playback operation from one media URI to the next in the list.
	/// </summary>
	public class PlaybackContinuingEvent : EventModel
	{
		/// <summary>
		/// Playback control object
		/// </summary>
		public PlaybackModel Playback { get; set; }
	}
}
