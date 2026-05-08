using AsteriskAriService.Session;

namespace AsteriskAriService.Models.Ari
{
    /// <summary>
    /// Limited by scope lifetime
    /// </summary>
    public class ClientSessionAri
    {
        /// <summary>
        /// Stasis parameters (args)
        /// </summary>
        public List<string> Parameters { get; set; }
        
        /// <summary>
        /// The one who initiated the call
        /// </summary>
        public ChannelAri Initiator { get; set; }

        /// <summary>
        /// lives only within the current Stasis scope
        /// </summary>
        public SessionStorage ScopeItems { get; } = new();
        
        /// <summary>
        /// lives between sessions for the current channel id.
        /// It will be transferred to the new session during the redirect.
        /// </summary>
        public SessionStorage CallItems { get; set; } = new();
        
    }
}