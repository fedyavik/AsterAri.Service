namespace AsteriskAriService.Bridges.Hold
{
    public class BridgeHoldModel(int holdTimer, string? moh = null, string? exitSound = null)
    {
        /// <summary>
        /// Music while waiting for a response
        /// </summary>
        public string? MusicOnHold { get; set; } = moh;
        
        /// <summary>
        /// Waiting time
        /// </summary>
        public int HoldTimer { get; set; } = holdTimer;
        
        /// <summary>
        /// Sound at the end of waiting
        /// </summary>
        /// <example>sound:en/file_name</example>>
        public string? SoundOnExit { get; set; } = exitSound;
    }
}