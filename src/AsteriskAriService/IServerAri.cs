using AsteriskAriService.Models.Number;

namespace AsteriskAriService
{
    public interface IServerAri
    {
        /// <summary>
        /// End the channel without throwing an error
        /// </summary>
        /// <returns></returns>
        public Task ChannelTryHangup(string channelId);
        
        /// <summary>
        /// Completing the bridge without throwing an error
        /// </summary>
        /// <param name="bridgeId"></param>
        /// <returns></returns>
        public Task BridgeTryHangup(string bridgeId);

        /// <summary>
        /// Redirecting the channel to the specified stasis handler
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="args"></param>
        /// <param name="redirectData">this data can be obtained by ClientSessionARI</param>
        public void StasisRedirect(string channelId, List<string> args, object? redirectData = null);

        /// <summary>
        /// Create a new call and send it to stasis
        /// </summary>
        /// <param name="srcNumber"></param>
        /// <param name="dstNumber"></param>
        /// <param name="stasisHandler"></param>
        /// <returns></returns>
        public Task Originate(AriNumber srcNumber, AriNumber dstNumber, string stasisHandler);

        /// <summary>
        /// Create a new call and send it to the specified context
        /// </summary>
        /// <param name="srcNumber"></param>
        /// <param name="context"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public Task Originate(AriNumber srcNumber,string context, string extension);
    }
}