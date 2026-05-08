using AsteriskAriService.Models.Ari;

namespace AsteriskAriService.Models.Exceptions
{
    public class RingTargetsEmptyException(ChannelAri source, IEnumerable<string> targets)
        : AsterAriException($"Couldn't create a call between {source.PhoneNumber} and ({string.Join(",", targets)})");
}