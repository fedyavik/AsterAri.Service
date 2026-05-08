using AsteriskAriService.Extensions;
using AsteriskAriService.Factories.Channels;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Models.Number;
using AsteriskAriService.Tools;
using Microsoft.Extensions.Logging;

namespace AsteriskAriService.Bridges.Ring
{
    /// <summary>
    /// He can call several participants until someone answers.
    /// Returns the channel who answer
    /// </summary>
    public class BridgeRing(BaseBridgeDependencies dep,
        ILogger<BridgeRing> logger,
        IChannelAriFactory channelAriFactory)
        : BaseBridgeAri(dep, logger)
    {
        private string? _musicOnHold;
        private int _holdTimer = 25;
        private List<AriNumber> _callNumbers = [];
        private List<ChannelAri> _ringChannels = [];

        /// <summary>
        /// Processing customer communication logic
        /// </summary>
        /// <returns> Returns the channel that answered the call first </returns>
        /// <exception cref="RingTargetsEmptyException">If the call list is empty</exception>
        /// <exception cref="AriTimeoutException">If you haven't reached us in the specified time</exception>
        /// <exception cref="ClientLeftException">The client has disconnected</exception>
        public async Task<ChannelAri> Handler(BridgeRingModel model)
        {
            SrcChannel = model.SrcChannelAri;
            _musicOnHold = model.MusicOnHold;
            _holdTimer = model.HoldTimer;
            _callNumbers = model.RingingEndpoints;

            await CreateChannels();
            var answer = await RingAll();
            Logger.LogInformation("{Number} answered the phone {ClientNumber}", answer.PhoneNumber, SrcChannel.PhoneNumber);
            await ReleaseBridge(answer);
            return answer;
        }

        private async Task CreateChannels()
        {
            await AddChannels(SrcChannel);
            foreach (var endpoint in _callNumbers)
            {
                var dstChannel = await channelAriFactory.TryCreateAsync(endpoint, originator: SrcChannel.Id);
                if (dstChannel is null)
                    continue;
                _ringChannels.Add(dstChannel);
                await TryAddChannels(dstChannel);
            }
            if (_ringChannels.Count == 0)
                throw new RingTargetsEmptyException(SrcChannel, _callNumbers.Select(x => x.Number));
        }
        /// <summary>
        /// Call all available numbers
        /// </summary>
        /// <returns>Returns the first responder</returns>
        /// <exception cref="AriTimeoutException"></exception>
        /// <exception cref="ClientLeftException"></exception>
        private async Task<ChannelAri> RingAll()
        {
            await SrcChannel.Answer();
            await StartMoh(_musicOnHold);
            
            var ringsDst = string.Join(',', _ringChannels.Select(x => x.PhoneNumber));
            Logger.LogInformation("Client connection {Number} {ChannelId} to {RingsList}",
                SrcChannel.PhoneNumber,SrcChannel.Id, ringsDst);
            
            foreach (var ring in _ringChannels)
                await ring.Call(SrcChannel.Id, _holdTimer + 1);
            
            return await WaitTools.WaitRingAnswer(SrcChannel, _holdTimer, _ringChannels.ToArray());
        }
        
        /// <summary>
        /// Clearing the remaining calls
        /// </summary>
        /// <param name="answer">In order not to clear the channel of the respondent</param>
        private async Task ReleaseBridge(ChannelAri answer)
        {
            await RemoveChannels(SrcChannel, answer);
            await Hangup();
        }
        /// <inheritdoc/>
        public override string BridgeType() => "mixing";
        /// <inheritdoc/>
        public override string BridgeName() => "AppRingBridge";
    }
}
