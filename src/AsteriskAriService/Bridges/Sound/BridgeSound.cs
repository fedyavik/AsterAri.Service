using AsteriskAriService.Extensions;
using AsteriskAriService.Factories.Playbacks;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Tools;
using Microsoft.Extensions.Logging;

namespace AsteriskAriService.Bridges.Sound
{
    /// <summary>
    /// To play the sound
    /// </summary>
    public class BridgeSound(BaseBridgeDependencies dep,
        ILogger<BridgeSound> logger,
        IPlaybackFactory playbackFactory)
        : BaseBridgeAri(dep, logger)
    {
        /// <summary>
        /// Audio playback logic processing
        /// </summary>
        /// <returns></returns>
        /// <param name="sound">sound:en/file_name</param>
        /// <exception cref="BridgeAddChannelException"> If the client disconnects before adding </exception>
        /// <exception cref="ClientLeftException"> If the client disconnects without listening to the audio </exception>
        public async Task Handler(string sound)
        {
            await AddChannels(SrcChannel);
            await SrcChannel.Answer();
            Logger.LogInformation("Play the message {Sound} to {Number}", sound, SrcChannel.PhoneNumber);
            var currentPlayback = await playbackFactory.CreatePlayback(SrcChannel, sound);

            var finishedListen = await WaitTools.WaitChannelListenPlayback(currentPlayback, SrcChannel);
            if (finishedListen)
            {
                Logger.LogInformation("Audio {Sound} completed for the client {Number}", sound, SrcChannel.PhoneNumber);
                return;
            }
            Logger.LogInformation("The client {Number} did not wait for the introduction to be completed", SrcChannel.PhoneNumber);
            throw new ClientLeftException(SrcChannel);
        }
        /// <inheritdoc/>
        public override string BridgeType() => "holding";
        /// <inheritdoc/>
        public override string BridgeName() => "AppPlaybackBridge";
    }
}