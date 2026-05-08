namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// User-generated event with additional user-defined fields in the object.
	/// </summary>
	public class ChannelUsereventEvent : EventModel
	{
		/// <summary>
		/// The name of the user event.
		/// </summary>
		public string Eventname { get; set; }

		/// <summary>
		/// A channel that is signaled with the user event.
		/// </summary>
		public ChannelModel? Channel { get; set; }

		/// <summary>
		/// A bridge that is signaled with the user event.
		/// </summary>
		public BridgeModel? Bridge { get; set; }

		/// <summary>
		/// A endpoint that is signaled with the user event.
		/// </summary>
		public EndpointModel? Endpoint { get; set; }

		/// <summary>
		/// Custom Userevent data
		/// </summary>
		public object Userevent { get; set; }
	}
}
