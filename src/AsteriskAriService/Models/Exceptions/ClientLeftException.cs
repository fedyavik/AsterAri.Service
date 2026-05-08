using AsteriskAriService.Models.Ari;

namespace AsteriskAriService.Models.Exceptions
{
    public class ClientLeftException(ChannelAri channel) : AsterAriException($"The client has disconnected:{channel.PhoneNumber}");
}