namespace AsteriskAriService.Models.Asterisk
{
    public class SoundModel
    {
        /// <summary>
        /// Sound's identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Text description of the sound, usually the words spoken.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The formats and languages in which this sound is available.
        /// </summary>
        public List<FormatLangPairModel> Formats { get; set; }
    }
}