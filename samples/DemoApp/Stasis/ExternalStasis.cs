using AsteriskAriService.Bridges.Ring;
using AsteriskAriService.Bridges.Sound;
using AsteriskAriService.Bridges.Talk;
using AsteriskAriService.Factories.Bridges;
using AsteriskAriService.Models;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Number;

namespace DemoApp.Stasis
{
    /// <summary>
    /// Processing business logic for incoming calls
    /// </summary>
    /// <param name="logger"></param>
    public class ExternalStasis(
        ILogger<ExternalStasis> logger,
        ClientSessionAri clientSessionAri,
        IBridgeFactory bridgeFactory
        ) : StasisHandler
    {
        private BridgeTalk? TalkState { get; set; } = null;
        private ChannelAri ClientChannel { get; set; }
        private ChannelAri? AnswerChannel { get; set; }
        /// <summary>
        /// Processing a new stasis connection
        /// </summary>
        public override async Task Handler()
        {
            ClientChannel = clientSessionAri.Initiator;
            logger.LogInformation("A new call from a client {Number} {ChannelId}", ClientChannel.PhoneNumber, ClientChannel.Id);
            
            await IntroStep();
            AnswerChannel = await CallStep();
            await TalkStep();
        }
        
        /// <summary>
        /// Playing the intro to the client
        /// </summary>
        private async Task IntroStep()
        {
            var clientHelloSound = "sound:en/hello";
            var soundState = await bridgeFactory.CreateBridgeAsync<BridgeSound>();
            await soundState.Handler(clientHelloSound);
        }
        
        /// <summary>
        /// We are calling all managers.
        /// </summary>
        /// <returns>Returns the responding manager</returns>
        private async Task<ChannelAri> CallStep()
        {
            var number1 = new PjsipNumber("3333");
            var number2 = new PjsipNumber("3334");
            var ringState = await bridgeFactory.CreateBridgeAsync<BridgeRing>();
            var ringModel = new BridgeRingModel(ClientChannel,[number1, number2]);
            return await ringState.Handler(ringModel);
        }
        
        /// <summary>
        /// The client's conversation with the manager
        /// </summary>
        /// <returns>Returns when the conversation is over</returns>
        private async Task TalkStep()
        {
            TalkState = await bridgeFactory.CreateBridgeAsync<BridgeTalk>();
            var model = new BridgeTalkModel(ClientChannel, AnswerChannel!, true);
            await TalkState.Handler(model);
        }
    }
}