namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// The state of a contact on an endpoint has changed.
	/// </summary>
	public class ContactStatusChangeEvent : EventModel
	{
		/// <summary>
		/// 
		/// </summary>
		public EndpointModel Endpoint { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ContactInfoModel Contact_info { get; set; }
	}
}
