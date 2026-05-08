namespace AsteriskAriService.Models.Asterisk
{
    /// <summary>
    /// Detailed information about a remote peer that communicates with Asterisk
    /// </summary>
    public class PeerModel
    {
        /// <summary>
        /// The current state of the peer.
        /// Note that the values of the status are dependent on the underlying peer technology.
        /// </summary>
        /// <returns></returns>
        public string Peer_status { get; set; }
        
        /// <summary>
        /// An optional reason associated with the change in peer_status.
        /// </summary>
        /// <returns></returns>
        public string? Cause { get; set; }
        
        /// <summary>
        /// The IP address of the peer.
        /// </summary>
        /// <returns></returns>
        public string? Address { get; set; }
        
        /// <summary>
        /// The port of the peer.
        /// </summary>
        /// <returns></returns>
        public string? Port { get; set; }
        
        /// <summary>
        /// The last known time the peer was contacted.
        /// </summary>
        /// <returns></returns>
        public string? Time { get; set; }
    }
}