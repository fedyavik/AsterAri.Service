namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Event showing the completion of a recording operation.
	/// </summary>
	public class RecordingFinishedEvent : EventModel
	{
		/// <summary>
		/// Recording control object
		/// </summary>
		public LiveRecordingModel Recording { get; set; }
	}
}
