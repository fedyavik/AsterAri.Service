namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// A channel initiated a media unhold.
	/// </summary>
	public class ChannelUnholdEvent : EventModel
	{
		/// <summary>
		/// The channel that initiated the unhold event.
		/// </summary>
		public ChannelModel Channel { get; set; }
	}
}
