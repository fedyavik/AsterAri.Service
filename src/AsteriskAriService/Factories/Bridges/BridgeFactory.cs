using AsteriskAriService.Actions.Bridge;
using AsteriskAriService.Bridges;
using AsteriskAriService.Models.Ari;
using Microsoft.Extensions.DependencyInjection;

namespace AsteriskAriService.Factories.Bridges
{
    public class BridgeFactory (IServiceProvider serviceProvider,
        IBridgeAriActions bridgeAriActions,
        ClientSessionAri clientSessionAri) 
        : IBridgeFactory
    {
        
        public async Task<T> CreateBridgeAsync<T>() where T : BaseBridgeAri
        {
            var bridge = serviceProvider.GetRequiredService<T>();
            await bridgeAriActions.CreateAsync(bridge.BridgeType(), bridge.BridgeId, 
                $"{bridge.BridgeName()}-{clientSessionAri.Initiator.Id}");
            return bridge;
        }
    }
}