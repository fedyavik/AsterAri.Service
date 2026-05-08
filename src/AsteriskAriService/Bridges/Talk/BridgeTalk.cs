using AsteriskAriService.Extensions;
using AsteriskAriService.Factories.Recordings;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.SessionInfo;
using AsteriskAriService.Tools;
using Microsoft.Extensions.Logging;

namespace AsteriskAriService.Bridges.Talk
{
    /// <summary>
    /// The conversation of the two, waiting for their conversation to end
    /// </summary>
    public class BridgeTalk(BaseBridgeDependencies dep,
        ILogger<BridgeTalk> logger,
        IRecordingFactory recordingFactory)
        : BaseBridgeAri(dep, logger)
    {
        private ChannelAri _dstChannel;
        private TalkInfo _talkInfo = new ();
        
        /// <summary>
        /// Conversation Logic processing
        /// </summary>
        /// <returns></returns>
        public async Task Handler(BridgeTalkModel model)
        {
            SrcChannel = model.SrcChannelAri;
            _dstChannel = model.DstChannelAri;
            _talkInfo.Answer = _dstChannel;
            ClientSession.ScopeItems.Set(_talkInfo);
            if (model.Recording)
            {
                var rec = await recordingFactory.StartRecordingAri(this, SrcChannel);
                _talkInfo.RecordingName = rec.RecordingName;
            }
            await Dialog();
        }

        private async Task Dialog()
        {
            await AddChannels(SrcChannel,_dstChannel);
            var startTime = DateTime.UtcNow;
            
            var channelLeft = await WaitTools.WhenAnyChannelLeft(SrcChannel, _dstChannel);
            if (channelLeft == SrcChannel)
                Logger.LogInformation("{ClientNumber} ended the conversation with {AnswerNumber}", SrcChannel.PhoneNumber, _dstChannel.PhoneNumber);
            else
                Logger.LogInformation("{AnswerNumber} ended the conversation with {ClientNumber}", _dstChannel.PhoneNumber, SrcChannel.PhoneNumber);
            
            _talkInfo.TalkDurationInSeconds = (float)(DateTime.UtcNow - startTime).TotalSeconds;
        }
        /// <inheritdoc/>
        public override string BridgeType() => "mixing";
        /// <inheritdoc/>
        public override string BridgeName() => "AppTalkBridge";
    }
}