namespace AsteriskAriService.Models.Asterisk
{
    /// <summary>
    /// Information about the requested destination
    /// </summary>
    public class RequiredDestinationModel
    {
        /// <summary>
        /// the requested protocol-id by the referee in case of SIP channel,
        /// this is a SIP Call ID, Mutually exclusive to destination
        /// </summary>
        public string? Protocol_id { get; set; }
        
        /// <summary>
        /// Destination User Part. Only for Blind transfer. Mutually exclusive to protocol_id
        /// </summary>
        public string? Destination { get; set; }
        
        /// <summary>
        /// List of additional protocol specific information
        /// </summary>
        public List<AdditionalParamModel>? Additional_protocol_params { get; set; }
    }
}