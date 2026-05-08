using AsteriskAriService.Models.Ari;

namespace AsteriskAriService.Bridges.Talk
{
    /// <summary>
    /// The parameter model for connecting subscribers
    /// </summary>
    /// <param name="srcChannelAri"></param>
    /// <param name="dstChannelAri"></param>
    /// <param name="useRecording"></param>
    public class BridgeTalkModel(ChannelAri srcChannelAri, ChannelAri dstChannelAri, bool useRecording = false)
    {
        /// <summary>
        /// Conversation initiator
        /// </summary>
        public ChannelAri SrcChannelAri { get; set; } = srcChannelAri;
        
        /// <summary>
        /// Who are we contacting the client with
        /// </summary>
        public ChannelAri DstChannelAri { get; set; } = dstChannelAri;

        /// <summary>
        /// Record a conversation?
        /// </summary>
        public bool Recording { get; set; } = useRecording;
    }
}