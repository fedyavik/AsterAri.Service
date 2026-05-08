using AsteriskAriService.Models.Ari;
using AsteriskAriService.Websocket;
using Microsoft.Extensions.Logging;

namespace AsteriskAriService.DtmfHandlers
{
    public class LogDtmfHandler(IWsAriClient wsAriClient, ILogger<LogDtmfHandler> logger) : BaseDtmfHandler(wsAriClient)
    {
        protected override void CodeSequenceHandler(ChannelAri channelAri, string sequence)
        {
            logger.LogInformation("The sequence is entered {Sequence}", sequence);
        }
    }
}