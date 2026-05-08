using AsteriskAriService.Actions.Bridge;
using AsteriskAriService.Models.Ari;
using Microsoft.Extensions.Logging;

namespace AsteriskAriService.Bridges
{
    /// <summary>
    /// Limited by scope lifetime
    /// </summary>
    public abstract class BaseBridgeAri: IAsyncDisposable, IDisposable
    {
        public string BridgeId { get; } = Guid.NewGuid().ToString();
        /// <summary>
        /// The channel that initiated the state
        /// </summary>
        protected ChannelAri SrcChannel { get; set; }
        protected ClientSessionAri ClientSession { get; }
        protected ILogger Logger { get; }
        private IBridgeAriActions BridgeAriActions { get; }
        private bool IsDisposed { get; set; }

        protected BaseBridgeAri(BaseBridgeDependencies dep,ILogger logger)
        {
            Logger = logger;
            BridgeAriActions = dep.BridgeAriActions;
            ClientSession = dep.ClientSessionAri;
            SrcChannel = ClientSession.Initiator;
        }
        
        /// <summary>
        /// Add music while waiting for a response
        /// </summary>
        protected async Task StartMoh(string? musicClass = null)
        {
            await BridgeAriActions.StartMohAsync(BridgeId, musicClass);
        }
        
        /// <summary>
        /// Turn off music while waiting for a response
        /// </summary>
        protected async Task StopMoh()
        {
            await BridgeAriActions.StopMohAsync(BridgeId);
        }
        
        /// <summary>
        /// Connect the channel to the bridge.
        /// The channel will be automatically deleted from the other bridge.
        /// </summary>
        /// <param name="channels"></param>
        protected virtual async Task AddChannels(params ChannelAri[] channels)
        {
            var ids = channels.Select(x => x.Id).ToArray();
            await BridgeAriActions.AddChannelAsync(BridgeId, channelsIds: ids);
        }
        
        /// <inheritdoc cref="AddChannels"/>
        protected async Task TryAddChannels(params ChannelAri[] channels)
        {
            try
            {
                await AddChannels(channels);
            }
            catch (Exception)
            {
                //ignored
            }
        }
        /// <summary>
        /// Disconnect the channel from the bridge
        /// </summary>
        /// <param name="channels"></param>
        public virtual async Task RemoveChannels(params ChannelAri[] channels)
        {
            var ids = channels.Select(x => x.Id).ToArray();
            await BridgeAriActions.TryRemoveChannelAsync(BridgeId, ids);
        }
        /// <summary>
        /// Remove the bridge and terminate all connected channels
        /// </summary>
        public virtual async Task Hangup()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            await BridgeAriActions.HangupConnectedChannelsAsync(BridgeId);
            await BridgeAriActions.TryDestroyAsync(BridgeId);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await Hangup();
            GC.SuppressFinalize(this);
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            Hangup().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// mixing, holding, dtmf_events, proxy_media, video_sfu
        /// </summary>
        /// <returns></returns>
        public abstract string BridgeType();
        
        /// <summary>
        /// Name to give to the bridge being created.
        /// </summary>
        /// <returns></returns>
        public abstract string BridgeName();
    }
}
