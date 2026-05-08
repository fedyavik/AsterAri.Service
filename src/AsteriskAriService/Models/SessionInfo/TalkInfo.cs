using AsteriskAriService.Models.Ari;

namespace AsteriskAriService.Models.SessionInfo
{
    public class TalkInfo
    { 
        /// <summary>
        /// The one who answered the call
        /// </summary>
        public ChannelAri? Answer { get; set; }
        /// <summary>
        /// The name of the conversation recording
        /// </summary>
        public string? RecordingName { get; set; }
        /// <summary>
        /// Conversation duration
        /// </summary>
        public float? TalkDurationInSeconds { get; set; }
    }
}