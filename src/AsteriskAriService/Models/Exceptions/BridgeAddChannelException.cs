namespace AsteriskAriService.Models.Exceptions
{
    public class BridgeAddChannelException(string channelId)
        : AsterAriException($"Couldn't add channel to bridge:{channelId}");
}