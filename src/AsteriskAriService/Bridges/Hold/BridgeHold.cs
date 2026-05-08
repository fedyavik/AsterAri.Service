using AsteriskAriService.Bridges.Sound;
using AsteriskAriService.Extensions;
using AsteriskAriService.Factories.Bridges;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Tools;
using Microsoft.Extensions.Logging;

namespace AsteriskAriService.Bridges.Hold
{
    /// <summary>
    /// Turn on looped music for the client to wait
    /// </summary>
    public class BridgeHold(
        BaseBridgeDependencies dep,
        ILogger<BridgeHold> logger,
        IBridgeFactory bridgeFactory)
        : BaseBridgeAri(dep, logger)
    {
        private string? MusicOnHold { get; set; }
        private string? ExitSound { get; set; }
        private int HoldTimer { get; set; }

        /// <summary>
        /// Processing the waiting logic
        /// </summary>
        /// <returns></returns>
        public async Task Handler(BridgeHoldModel model)
        {
            MusicOnHold = model.MusicOnHold;
            ExitSound = model.SoundOnExit;
            HoldTimer = model.HoldTimer;
            
            await AddChannels(SrcChannel);
            await SrcChannel.Answer();
            await StartMoh(MusicOnHold);
            await HoldTimerWithRedirect(HoldTimer);
        }

        /// <summary>
        /// Waiting time in the channel and redirection to completion
        /// </summary>
        private async Task HoldTimerWithRedirect(int delay)
        {
            var channelLeft = await WaitTools.WaitChannelLeftOrTimeout(SrcChannel, delay);
            if (channelLeft == SrcChannel)
                return;
            if (ExitSound is null)
                return;
            var soundState = await bridgeFactory.CreateBridgeAsync<BridgeSound>();
            await soundState.Handler(ExitSound);
        }
        /// <inheritdoc/>
        public override string BridgeType() => "holding";
        /// <inheritdoc/>
        public override string BridgeName() => "AppHoldBridge";
    }
}