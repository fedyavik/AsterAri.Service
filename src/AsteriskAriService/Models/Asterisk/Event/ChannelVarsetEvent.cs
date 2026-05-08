namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Channel variable changed.
	/// </summary>
	public class ChannelVarsetEvent : EventModel
	{
		/// <summary>
		/// The variable that changed.
		/// </summary>
		public string Variable { get; set; }

		/// <summary>
		/// The new value of the variable.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// The channel on which the variable was set.
		/// If missing, the variable is a global variable.
		/// </summary>
		public ChannelModel? Channel { get; set; }
	}
}
