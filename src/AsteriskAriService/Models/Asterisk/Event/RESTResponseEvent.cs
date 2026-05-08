namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// REST over Websocket Response
	/// </summary>
	public class RESTResponseEvent  : EventModel
	{
		/// <summary>
		/// Opaque transaction id.
		/// Will be whatever was specified on the original request.
		/// </summary>
		public string Transaction_id { get; set; }
		
		/// <summary>
		/// Opaque request id.
		/// Will be whatever was specified on the original request.
		/// </summary>
		public string Request_id { get; set; }
		
		/// <summary>
		/// HTTP status code
		/// </summary>
		public int Status_code { get; set; }
		
		/// <summary>
		/// HTTP reason phrase
		/// </summary>
		public string Reason_phrase { get; set; }
		
		/// <summary>
		/// Original request resource URI
		/// </summary>
		public string Uri { get; set; }
		
		/// <summary>
		/// The Content-Type of the message body
		/// </summary>
		public string? Content_type { get; set; }
		
		/// <summary>
		/// Response message body
		/// </summary>
		public string? Message_body { get; set; }
	}
}
