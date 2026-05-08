namespace AsteriskAriService.Models.Asterisk.AsteriskInfo
{
    public class AsteriskInfoModel
    {
        /// <summary>
        /// Info about how Asterisk was built
        /// </summary>
        public BuildInfoModel Build { get; set; }

        /// <summary>
        /// Info about the system running Asterisk
        /// </summary>
        public SystemInfoModel System { get; set; }

        /// <summary>
        /// Info about Asterisk configuration
        /// </summary>
        public ConfigInfoModel Config { get; set; }

        /// <summary>
        /// Info about Asterisk status
        /// </summary>
        public StatusInfoModel Status { get; set; }
    }
}