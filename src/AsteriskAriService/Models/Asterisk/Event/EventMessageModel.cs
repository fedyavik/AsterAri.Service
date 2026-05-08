namespace AsteriskAriService.Models.Asterisk.Event
{
    public class MessageModel
    {
        /// <summary>
        /// Indicates the type of this message.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The unique ID for the Asterisk instance that raised this event.
        /// </summary>
        public string Asterisk_id { get; set; }
    }
}