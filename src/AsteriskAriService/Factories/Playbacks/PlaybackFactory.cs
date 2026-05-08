using AsteriskAriService.Actions.Channel;
using AsteriskAriService.Models.Ari;
using Microsoft.Extensions.DependencyInjection;

namespace AsteriskAriService.Factories.Playbacks
{
    public class PlaybackFactory(IServiceProvider sp,
        IChannelAriActions channelAriActions) : IPlaybackFactory
    {
        public async Task<PlaybackAri> CreatePlayback(ChannelAri channel, string mediaUrl)
        {
            var model =  await channelAriActions.PlayAsync(channel.Id, mediaUrl);
            var ariModel = sp.GetRequiredService<PlaybackAri>();
            ariModel.Init(model);
            return ariModel;
        }
    }
}