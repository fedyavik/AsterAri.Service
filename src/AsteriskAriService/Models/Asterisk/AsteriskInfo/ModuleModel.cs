namespace AsteriskAriService.Models.Asterisk.AsteriskInfo
{
    public class ModuleModel
    {
        /// <summary>
        /// The name of this module
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of this module
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The number of times this module is being used
        /// </summary>
        public int Use_count { get; set; }

        /// <summary>
        /// The running status of this module
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The support state of this module
        /// </summary>
        public string Support_level { get; set; }
    }
}