namespace AsteriskAriService.Models.Asterisk
{
    public class EndpointModel
    {
        /// <summary>
        /// Technology of the endpoint
        /// </summary>
        public string Technology { get; set; }

        /// <summary>
        /// Identifier of the endpoint, specific to the given technology.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Endpoint's state
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Id's of channels associated with this endpoint
        /// </summary>
        public List<string> Channel_ids { get; set; }
    }
}