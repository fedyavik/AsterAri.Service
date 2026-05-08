namespace AsteriskAriService.Models.Asterisk.AsteriskInfo
{
    public class LogChannelModel
    {
        /// <summary>
        /// The log channel path
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Types of logs for the log channel
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Whether or not a log type is enabled
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The various log levels
        /// </summary>
        public string Configuration { get; set; }
    }
}