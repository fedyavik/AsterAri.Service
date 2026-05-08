namespace AsteriskAriService.Models.Asterisk
{
    public class AsteriskPingModel
    {
        /// <summary>
        /// Asterisk id info
        /// </summary>
        public string Asterisk_id { get; set; }

        /// <summary>
        /// Always string value is pong
        /// </summary>
        public string Ping { get; set; }

        /// <summary>
        /// The timestamp string of request received time
        /// </summary>
        public string Timestamp { get; set; }
    }
}