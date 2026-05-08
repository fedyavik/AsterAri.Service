namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// A text message was received from an endpoint.
	/// </summary>
	public class TextMessageReceivedEvent : EventModel
	{
		/// <summary>
		/// no description provided
		/// </summary>
		public TextMessageModel Message { get; set; }

		/// <summary>
		/// no description provided
		/// </summary>
		public EndpointModel? Endpoint { get; set; }

	}
}
