using AsteriskAriService.Bridges.Ring;
using AsteriskAriService.Bridges.Talk;
using AsteriskAriService.Factories.Bridges;
using AsteriskAriService.Models;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Number;

namespace DemoApp.Stasis
{
    public class DefaultStasis(
        ClientSessionAri clientSessionAri,
        IBridgeFactory bridgeFactory
        ) : StasisHandler
    {
        private ChannelAri ClientChannel { get; set; }
        private ChannelAri? AnswerChannel { get; set; }
        
        /// <inheritdoc />
        public override async Task Handler()
        {
            /*
             * Parameters[0] = "Call"
             * Parameters[1] = ${EXTEN}
             * by n,Stasis(TestARI,"Call",${EXTEN})
             */
            var dstNumber = clientSessionAri.Parameters[1];
            ClientChannel = clientSessionAri.Initiator;
            var callEndpoint = AriNumber.Parse(dstNumber);
            
            /*
             * We call the specified number and, if successful, receive its channel.
             */
            AnswerChannel = await RingStep(callEndpoint);
            /*
             * Combining channels for conversation
             */
            await TalkStep();
        }

        private async Task<ChannelAri> RingStep(AriNumber callEndpoint)
        {
            var bridgeModel = new BridgeRingModel(ClientChannel, [callEndpoint], hold:45);
            var ringState = await bridgeFactory.CreateBridgeAsync<BridgeRing>();
            return await ringState.Handler(bridgeModel);
        }
        private async Task TalkStep()
        {
            var model = new BridgeTalkModel(ClientChannel, AnswerChannel!, true);
            var talkState = await bridgeFactory.CreateBridgeAsync<BridgeTalk>();
            await talkState.Handler(model);
        }
    }
}