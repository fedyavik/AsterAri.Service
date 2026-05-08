using AsteriskAriService;
using AsteriskAriService.DtmfHandlers;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Websocket;

namespace DemoApp.Dtmf
{
    public class TalkDtmfHandler(IServerAri serverAri, IWsAriClient wsAriClient) : BaseDtmfHandler(wsAriClient)
    {
        private ChannelAri _client;

        /// <summary>
        /// Start listening to commands
        /// </summary>
        /// <param name="client">The channel to which the actions will be applied</param>
        /// <param name="listen">Channels where we will listen to the codes</param>
        public async Task Start(ChannelAri client, params ChannelAri[] listen)
        {
            _client = client;
            await base.Start(listen);
        }
        protected override void CodeSequenceHandler(ChannelAri channelAri, string sequence)
        {   
            /*
             * If any listening channel dials the sequence *1110*,
             * then the _client channel will be redirected to the "Call" stasis processing with the "1110" parameter.
             */
            if (sequence == "1110")
            {
                serverAri.StasisRedirect(_client.Id, ["Call", "1110"]); 
                return;
            }
            base.CodeSequenceHandler(channelAri, sequence);
        }
    }
}