using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Number;

namespace AsteriskAriService.Bridges.Ring
{
    /// <summary>
    /// The parameter model for connecting subscribers
    /// </summary>
    /// <param name="srcChannelAri"></param>
    /// <param name="ringingEndpoints"></param>
    /// <param name="moh"></param>
    public class BridgeRingModel(ChannelAri srcChannelAri, List<AriNumber> ringingEndpoints, string? moh = null, int hold = 30)
    {
        /// <summary>
        /// Conversation initiator
        /// </summary>
        public ChannelAri SrcChannelAri { get; set; } = srcChannelAri;
        
        /// <summary>
        /// Who are we contacting the client with
        /// </summary>
        public List<AriNumber> RingingEndpoints { get; set; } = ringingEndpoints;
        
        /// <summary>
        /// Music while waiting for a response
        /// </summary>
        public string? MusicOnHold { get; set; } = moh;

        /// <summary>
        /// Response waiting time
        /// </summary>
        public int HoldTimer { get; set; } = hold;
    }
}