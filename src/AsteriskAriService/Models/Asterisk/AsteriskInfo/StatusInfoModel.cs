namespace AsteriskAriService.Models.Asterisk.AsteriskInfo
{
    public class StatusInfoModel
    {
        /// <summary>
        /// Time when Asterisk was started.
        /// </summary>
        public DateTime Startup_time { get; set; }

        /// <summary>
        /// Time when Asterisk was last reloaded.
        /// </summary>
        public DateTime Last_reload_time { get; set; }
    }
}