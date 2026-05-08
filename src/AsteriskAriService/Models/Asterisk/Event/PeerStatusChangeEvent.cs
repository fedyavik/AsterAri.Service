namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// The state of a peer associated with an endpoint has changed.
	/// </summary>
	public class PeerStatusChangeEvent : EventModel
	{
		/// <summary>
		/// 
		/// </summary>
		public EndpointModel Endpoint { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public PeerModel Peer { get; set; }
	}
}
