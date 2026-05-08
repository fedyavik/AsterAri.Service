using AsteriskAriService.Models.Ari;

namespace AsteriskAriService.Factories.Playbacks
{
    public interface IPlaybackFactory
    {
        /// <summary>
        /// Play audio on a channel
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mediaUrl">Media URIs to play. Ex: sound:en/file_name</param>
        /// <returns></returns>
        public Task<PlaybackAri> CreatePlayback(ChannelAri channel, string mediaUrl);
    }
}