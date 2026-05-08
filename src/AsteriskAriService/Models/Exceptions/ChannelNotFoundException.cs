namespace AsteriskAriService.Models.Exceptions
{
    public class ChannelNotFoundException(string channelId) : AsterAriException($"Couldn't find the channel: {channelId}");
}