namespace AsteriskAriService.Models.Asterisk
{
    public class LiveRecordingModel
    {
        /// <summary>
        /// Base name for the recording
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Recording format (wav, gsm, etc.)
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// URI for the channel or bridge being recorded
        /// </summary>
        public string Target_uri { get; set; }
        /// <summary>
        /// ["queued","recording","paused","done","failed","canceled"]
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Duration in seconds of the recording
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// Duration of talking, in seconds, detected in the recording.
        /// This is only available if the recording was initiated with a non-zero maxSilenceSeconds.
        /// </summary>
        public int Talking_duration { get; set; }
        /// <summary>
        /// Duration of silence, in seconds, detected in the recording.
        /// This is only available if the recording was initiated with a non-zero maxSilenceSeconds.
        /// </summary>
        public int Silence_duration { get; set; }
        /// <summary>
        /// Cause for recording failure if failed
        /// </summary>
        public string? Cause { get; set; }
    }
}