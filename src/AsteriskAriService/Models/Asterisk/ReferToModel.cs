namespace AsteriskAriService.Models.Asterisk
{
    /// <summary>
    /// transfer destination requested by transferee
    /// </summary>
    public class ReferToModel
    {
        /// <summary>
        /// 
        /// </summary>
        public RequiredDestinationModel Requested_destination { get; set; }

        /// <summary>
        /// The Channel Object, that is to be replaced
        /// </summary>
        public ChannelModel Destination_channel { get; set; }
        
        /// <summary>
        /// Channel, connected to the to be replaced channel
        /// </summary>
        public ChannelModel Connected_channel { get; set; }
        
        /// <summary>
        /// Bridge connecting both destination channels
        /// </summary>
        public BridgeModel Bridge { get; set; }
    }
}