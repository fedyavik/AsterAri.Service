namespace AsteriskAriService.Models.Asterisk.Event
{
	/// <summary>
	/// Notification that a device state has changed.
	/// </summary>
	public class DeviceStateChangedEvent : EventModel
	{
		/// <summary>
		/// Device state object
		/// </summary>
		public DeviceStateModel Device_state { get; set; }
	}
}
