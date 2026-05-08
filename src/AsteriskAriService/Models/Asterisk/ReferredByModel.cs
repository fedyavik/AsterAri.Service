namespace AsteriskAriService.Models.Asterisk
{
    /// <summary>
    /// transfer destination requested by transferee
    /// </summary>
    public class ReferredByModel
    {
        /// <summary>
        /// The channel on which the refer was received
        /// </summary>
        public ChannelModel source_channel { get; set; }
     
        /// <summary>
        /// Channel, Connected to the channel, receiving the transfer request on
        /// </summary>
        public ChannelModel? Connected_channel { get; set; }
        
        /// <summary>
        /// Bridge connecting both Channels
        /// </summary>
        public BridgeModel? Bridge { get; set; }
    }
}