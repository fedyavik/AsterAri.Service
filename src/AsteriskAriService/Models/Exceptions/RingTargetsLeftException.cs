namespace AsteriskAriService.Models.Exceptions
{
    public class RingTargetsLeftException(IEnumerable<string> targets)
        : AsterAriException($"Everyone disconnected from the call ({string.Join(",", targets)})");
}