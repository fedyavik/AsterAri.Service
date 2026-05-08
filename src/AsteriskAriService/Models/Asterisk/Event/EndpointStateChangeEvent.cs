namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Endpoint state changed.
	/// </summary>
	public class EndpointStateChangeEvent : EventModel
	{
		/// <summary>
		/// no description provided
		/// </summary>
		public EndpointModel Endpoint { get; set; }
	}
}
