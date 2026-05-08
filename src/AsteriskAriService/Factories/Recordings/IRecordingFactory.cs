using AsteriskAriService.Bridges;
using AsteriskAriService.Models.Ari;

namespace AsteriskAriService.Factories.Recordings
{
    public interface IRecordingFactory
    {
        /// <summary>
        /// Start recording a conversation on the bridge
        /// </summary>
        /// <param name="bridge">The bridge to start recording on</param>
        /// <param name="channel">the channel where the recording information will be saved</param>
        /// <returns></returns>
        public Task<LiveRecordingAri> StartRecordingAri(BaseBridgeAri bridge, ChannelAri channel);
    }
}