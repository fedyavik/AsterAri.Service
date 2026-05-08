using AsteriskAriService.Bridges.Ring;
using AsteriskAriService.Bridges.Talk;
using AsteriskAriService.Factories.Bridges;
using AsteriskAriService.Factories.Dtmf;
using AsteriskAriService.Models;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Number;
using DemoApp.Dtmf;

namespace DemoApp.Stasis
{
    public class TestDtmfStasis(
        ILogger<TestDtmfStasis> logger,
        ClientSessionAri clientSessionAri,
        IBridgeFactory bridgeFactory,
        IDtmfFactory dtmfFactory
        ) : StasisHandler
    {
        private ChannelAri ClientChannel { get; set; }

        public override async Task Handler()
        {
            ClientChannel = clientSessionAri.Initiator;
            logger.LogInformation("The customer {Number} is calling 3333", ClientChannel.PhoneNumber);
            var answer = await RingStep();
            await TalkStep(answer);
        }

        private async Task<ChannelAri> RingStep()
        {
            var target = new PjsipNumber("3333");
            var ringState = await bridgeFactory.CreateBridgeAsync<BridgeRing>();
            var model = new BridgeRingModel(ClientChannel, [target],hold:45);
            return await ringState.Handler(model);
        }

        private async Task TalkStep(ChannelAri inspectorChannel)
        {
            using var dtmfHandler = dtmfFactory.CreateDtmfHandler<TalkDtmfHandler>();
            /*
             * We turn on listening to codes.
             * If you do not use using, then listening will work until scope is completed.
             */
            await dtmfHandler.Start(ClientChannel, inspectorChannel); 
            var model = new BridgeTalkModel(ClientChannel, inspectorChannel, true);
            var talkState = await bridgeFactory.CreateBridgeAsync<BridgeTalk>();
            await talkState.Handler(model);
        }
    }
}
