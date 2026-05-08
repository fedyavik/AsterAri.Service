namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Event showing failure of a recording operation.
	/// </summary>
	public class RecordingFailedEvent : EventModel
	{
		/// <summary>
		/// Recording control object
		/// </summary>
		public LiveRecordingModel Recording { get; set; }
	}
}
