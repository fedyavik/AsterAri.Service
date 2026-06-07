using System.Runtime.Serialization;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Models.Number
{
    [KnownType(typeof(SipNumber))]
    [KnownType(typeof(PjsipNumber))]
    public class AriNumber(string number)
    {
        public string Number { get; } = number;
        public string Endpoint { get; protected init; } = $"local/{number}@from-internal";

        public override string ToString() => Endpoint;
        private const int MobilePhoneLength = 10;

        public static AriNumber Parse(string numberOrEndpoint)
        {
            var parts = numberOrEndpoint.Split("/");
            if (parts.Length == 1)
                return new AriNumber(numberOrEndpoint);
            
            if (parts[0] == "PJSIP")
            {
                var numberAndTrunk = parts[1].Split("@");
                return numberAndTrunk.Length == 1 ? new PjsipNumber(parts[1]) : new PjsipNumber(numberAndTrunk[0], numberAndTrunk[1]);
            }

            if (parts[0] == "SIP")
                return parts.Length > 2 ? new SipNumber(parts[2], parts[1]) : new SipNumber(parts[1]);

            if (parts[0] == "local")
            {
                var numberAndTrunk = parts[1].Split("@");
                return new AriNumber(numberAndTrunk[0]);
            }
            
            throw new AsterAriException($"Unknown endpoint type {numberOrEndpoint}");
        }
    }
}