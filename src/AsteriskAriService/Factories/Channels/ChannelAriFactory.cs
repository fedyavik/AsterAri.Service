using AsteriskAriService.Actions.Channel;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Number;
using Microsoft.Extensions.DependencyInjection;

namespace AsteriskAriService.Factories.Channels
{
    public class ChannelAriFactory(
        IChannelAriActions channelActions,
        IServiceProvider sp
        ) : IChannelAriFactory
    {
        public async Task<ChannelAri?> TryCreateAsync(AriNumber endpoint, string? originator = null)
        {
            try
            {
                var model = await channelActions.CreateAsync(endpoint, originator: originator);
                var channelAri = sp.GetRequiredService<ChannelAri>();
                channelAri.Init(model, endpoint.Number);
                return channelAri;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ChannelAri> GetAsync(string channelId)
        {
            var model = await channelActions.GetAsync(channelId);
            var channelAri = sp.GetRequiredService<ChannelAri>();
            channelAri.Init(model, model.Caller.Number);
            return channelAri;
        }
    }
}