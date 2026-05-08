namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// transfer on a channel.
	/// </summary>
	public class ChannelTransferEvent : EventModel
	{
		/// <summary>
		/// Transfer State
		/// </summary>
		public string? State { get; set; }
		
		/// <summary>
		/// Refer-To information with optionally both affected channels
		/// </summary>
		public ReferToModel Refer_to { get; set; }
		
		/// <summary>
		/// Referred-By SIP Header according rfc3892
		/// </summary>
		public ReferredByModel Referred_by { get; set; }
	}
}
