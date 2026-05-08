using AsteriskAriService.Actions.Bridge;
using AsteriskAriService.Actions.Channel;
using AsteriskAriService.Middlewares;
using AsteriskAriService.Models;
using AsteriskAriService.Models.Asterisk.Event;
using AsteriskAriService.Models.Number;
using AsteriskAriService.Session;
using AsteriskAriService.Websocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AsteriskAriService
{
    public class ServerAri : IServerAri
    {
        private ILogger Logger { get; }
        private IWsAriClient WsAriClient { get; }
        private IServiceProvider ServiceProvider { get; }
        private AriConnectOptions AriOptions { get; }
        
        public ServerAri(IWsAriClient wsAriClient,IServiceProvider serviceProvider,
            IOptions<ServerAriOptions> options, ILogger<ServerAri> logger)
        {
            AriOptions = options.Value.ConnectOptions;
            ServiceProvider = serviceProvider;
            WsAriClient = wsAriClient;
            Logger = logger;
            WsAriClient.OnStasisStart.Subscribe(OnStasisStartEventHandler);
        }

        private void OnStasisStartEventHandler(StasisStartEvent e)
        {
            if (e.Application != AriOptions.AppName)
                return;
            if (e.Args.Count == 0)
                return;
            _ = StartSessionPipeline(e.Channel.Id, e.Args);
        }

        private async Task StartSessionPipeline(string channelId, List<string> args, SessionStorage? redirectData = null)
        {
            using var scope = ServiceProvider.CreateScope();
            var scopeServiceProvider = scope.ServiceProvider;
            var pipeline = ActivatorUtilities.CreateInstance<SessionPipeline>(scopeServiceProvider);
            await pipeline.Start(channelId, args, redirectData);
        }
        
        /// <inheritdoc/>>
        public async Task Originate(AriNumber srcNumber, AriNumber dstNumber, string stasisHandler)
        {
            var channelActions = ServiceProvider.GetRequiredService<IChannelAriActions>();
            var appName = AriOptions.AppName;
            await channelActions
                .OriginateAsync(srcNumber,
                    app: appName,
                    appArgs: $"{stasisHandler},{dstNumber.Endpoint}",
                    callerId: srcNumber.Number
                );
            Logger.LogInformation("A call was created between {NumberIn} {NumberOut}", srcNumber, dstNumber);
        }
        /// <inheritdoc/>>
        public async Task Originate(AriNumber srcNumber, string context, string extension)
        {
            var channelActions = ServiceProvider.GetRequiredService<IChannelAriActions>();
            await channelActions
                .OriginateAsync(srcNumber,
                    callerId: srcNumber.Number,
                    context:context,
                    extension:extension
                );
            Logger.LogInformation("A call has been created {NumberIn} to {Context} {Extension}", srcNumber, context,extension);
        }
        
        /// <inheritdoc/>>
        public async Task BridgeTryHangup(string bridgeId)
        {
            var bridgeActions = ServiceProvider.GetRequiredService<IBridgeAriActions>();
            await bridgeActions.TryDestroyAsync(bridgeId);
        }
        /// <inheritdoc/>
        public async Task ChannelTryHangup(string channelId)
        {
            var actions = ServiceProvider.GetRequiredService<IChannelAriActions>();
            await actions.TryHangupAsync(channelId);
        }

        /// <inheritdoc/>
        public void StasisRedirect(string channelId, List<string> args, object? redirectData = null)
        {
            var sessionManager = ServiceProvider.GetRequiredService<ISessionManager>();
            var session = sessionManager.GetSession(channelId);
            if (session is null)
            {
                SessionStorage? storage = null;
                if (redirectData is not null)
                {
                    storage = new SessionStorage();
                    storage.Set(redirectData);
                }
                _ = StartSessionPipeline(channelId, args, storage);
                Logger.LogInformation("Redirect {ChannelId} to {Args}", 
                    channelId, string.Join(",", args));
                return;
            }
            session.Initiator.DisposeWhenLeft();
            if (redirectData is not null)
                session.CallItems.Set(redirectData);
            _ = StartSessionPipeline(channelId, args, session.CallItems);
            
            Logger.LogInformation("Redirect {Number} {ChannelId} to {Args}", 
                session.Initiator.Caller.Number,channelId, string.Join(",", args));
        }
    }
}