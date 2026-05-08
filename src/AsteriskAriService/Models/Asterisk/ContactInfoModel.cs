namespace AsteriskAriService.Models.Asterisk
{
    /// <summary>
    /// Detailed information about a contact on an endpoint
    /// </summary>
    public class ContactInfoModel
    {
        /// <summary>
        /// The location of the contact
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The current status of the contact
        /// allowableValues:["Unreachable","Reachable","Unknown","NonQualified","Removed"]
        /// </summary>
        public string Contact_status { get; set; }

        /// <summary>
        /// The Address of Record this contact belongs to
        /// </summary>
        public string Aor { get; set; }

        /// <summary>
        /// Current round trip time, in microseconds, for the contact
        /// </summary>
        public string Roundtrip_usec { get; set; }
    }
}