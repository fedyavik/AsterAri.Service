namespace AsteriskAriService.Models.Exceptions
{
    public class ClientBusyException(string phone) : AsterAriException($"The number {phone} is busy and cannot receive the call.");
}