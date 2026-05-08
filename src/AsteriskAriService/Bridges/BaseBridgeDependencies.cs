using AsteriskAriService.Actions.Bridge;
using AsteriskAriService.Models;
using AsteriskAriService.Models.Ari;

namespace AsteriskAriService.Bridges
{
    public class BaseBridgeDependencies(
        IBridgeAriActions bridgesActions,
        ClientSessionAri client)
    {
        public IBridgeAriActions BridgeAriActions { get; private set; } = bridgesActions;
        public ClientSessionAri ClientSessionAri { get; private set; } = client;
    }
}