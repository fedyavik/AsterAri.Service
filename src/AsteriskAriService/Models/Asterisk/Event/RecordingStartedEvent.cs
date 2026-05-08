namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Event showing the start of a recording operation.
	/// </summary>
	public class RecordingStartedEvent : EventModel
	{
		/// <summary>
		/// Recording control object
		/// </summary>
		public LiveRecordingModel Recording { get; set; }
	}
}
