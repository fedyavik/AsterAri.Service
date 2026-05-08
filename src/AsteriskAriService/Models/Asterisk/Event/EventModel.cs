namespace AsteriskAriService.Models.Asterisk.Event
{
    public class EventModel : MessageModel
    {
        /// <summary>
        /// Name of the application receiving the event.
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Time at which this event was created.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Type of event
        /// </summary>
        public AriEventType EventType { get; set; }
    }
}