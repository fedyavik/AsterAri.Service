namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a channel has been destroyed.
	/// </summary>
	public class ChannelDestroyedEvent : EventModel
	{
		/// <summary>
		/// Integer representation of the cause of the hangup.
		/// </summary>
		public int Cause { get; set; }

		/// <summary>
		/// Text representation of the cause of the hangup.
		/// </summary>
		public string Cause_txt { get; set; }

		/// <summary>
		/// Integer representation of the technology-specific off-nominal cause of the hangup.
		/// </summary>
		public int? Tech_cause { get; set; }
		
		/// <summary>
		/// no description provided
		/// </summary>
		public ChannelModel Channel { get; set; }
	}
}
