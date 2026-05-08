using AsteriskAriService.Actions.Application;
using AsteriskAriService.Actions.Asterisk;
using AsteriskAriService.Actions.Bridge;
using AsteriskAriService.Actions.Channel;
using AsteriskAriService.Actions.DeviceStates;
using AsteriskAriService.Actions.Endpoint;
using AsteriskAriService.Actions.Event;
using AsteriskAriService.Actions.Mailbox;
using AsteriskAriService.Actions.Playback;
using AsteriskAriService.Actions.Recording;
using AsteriskAriService.Actions.Sounds;
using AsteriskAriService.Bridges;
using AsteriskAriService.Bridges.Hold;
using AsteriskAriService.Bridges.Ring;
using AsteriskAriService.Bridges.Sound;
using AsteriskAriService.Bridges.Talk;
using AsteriskAriService.DtmfHandlers;
using AsteriskAriService.Factories.Bridges;
using AsteriskAriService.Factories.Channels;
using AsteriskAriService.Factories.Dtmf;
using AsteriskAriService.Factories.Playbacks;
using AsteriskAriService.Factories.Recordings;
using AsteriskAriService.Factories.Session;
using AsteriskAriService.Http;
using AsteriskAriService.Middlewares;
using AsteriskAriService.Models;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Session;
using AsteriskAriService.Websocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AsteriskAriService.Extensions
{
    public static class AriServiceCollectionExtensions
    {
        public static IServiceCollection AddServerAri(this IServiceCollection services, Action<ServerAriOptions> setup)
        {
            var options = InitOptions(services, setup);

            RegisterSession(services);

            RegisterBridges(services, options);

            RegisterAriFactory(services);

            RegisterDtmf(services, options);

            RegisterStasisHandlers(services, options);

            RegisterMiddleware(services, options);
            
            services.RegisterActionsHttpClient(options.ConnectOptions);
            
            services.TryAddSingleton<IWsAriClient, WsClientAri>();
            services.ActivateSingleton<IWsAriClient>();
            
            services.TryAddSingleton<IServerAri, ServerAri>();
            services.ActivateSingleton<IServerAri>();
            return services;
        }

        private static ServerAriOptions InitOptions(IServiceCollection services,Action<ServerAriOptions> setup)
        {
            var options = new ServerAriOptions();
            setup.Invoke(options);
            services.Configure<ServerAriOptions>(opt =>
            {
                setup(opt);
                opt.Middlewares.Add(typeof(StasisAriMiddleware));
            });
            return options;
        }
        private static void RegisterSession(IServiceCollection services)
        {
            services.TryAddSingleton<ISessionManager, SessionManager>();
            services.TryAddTransient<ISessionFactory, SessionFactory>();
            services.TryAddScoped<ClientSessionAri>();
        }
        
        private static void RegisterBridges(IServiceCollection services, ServerAriOptions options)
        {
            services.TryAddTransient<BaseBridgeDependencies>();
            
            services.TryAddTransient<IBridgeFactory, BridgeFactory>();
            services.AddTransient<BridgeSound>();
            services.AddTransient<BridgeRing>();
            services.AddTransient<BridgeTalk>();
            services.AddTransient<BridgeHold>();
            foreach (var bridge in options.Bridges)
                services.AddTransient(bridge);
        }
        private static void RegisterAriFactory(IServiceCollection services)
        {
            services.TryAddTransient<IChannelAriFactory, ChannelAriFactory>();
            services.TryAddTransient<ChannelAri>();
            
            services.TryAddTransient<IRecordingFactory, RecordingFactory>();
            services.TryAddTransient<LiveRecordingAri>();
            
            services.TryAddTransient<IPlaybackFactory, PlaybackFactory>();
            services.TryAddTransient<PlaybackAri>();
        }
        private static void RegisterDtmf(IServiceCollection services, ServerAriOptions options)
        {
            services.TryAddTransient<IDtmfFactory, DtmfFactory>();
            services.AddTransient<BaseDtmfHandler>();
            services.AddTransient<LogDtmfHandler>();
            foreach (var handler in options.DtmfHandlers)
                services.AddTransient(handler);
        }

        private static void RegisterStasisHandlers(IServiceCollection services, ServerAriOptions options)
        {
            foreach (var handler in options.StasisHandlers)
                services.AddTransient(handler.Value);
        }
        
        private static void RegisterMiddleware(IServiceCollection services, ServerAriOptions options)
        {
            foreach (var middleware in options.Middlewares)
                services.AddTransient(middleware);
            services.TryAddTransient<StasisAriMiddleware>();
        }
        private static void RegisterActionsHttpClient(this IServiceCollection services, AriConnectOptions options)
        {
            services.AddAriHttpClient<IChannelAriActions, ChannelAriActions>(options);
            services.AddAriHttpClient<IBridgeAriActions, BridgeAriActions>(options);
            services.AddAriHttpClient<IRecordingsAriActions, RecordingsAriActions>(options);
            services.AddAriHttpClient<IApplicationAriActions, ApplicationAriActions>(options);
            services.AddAriHttpClient<IAsteriskAriActions, AsteriskAriActions>(options);
            services.AddAriHttpClient<IDeviceStatesAriActions, DeviceStatesAriActions>(options);
            services.AddAriHttpClient<IEndpointAriActions, EndpointAriActions>(options);
            services.AddAriHttpClient<IEventAriActions, EventAriActions>(options);
            services.AddAriHttpClient<IMailboxesAriActions, MailboxesAriActions>(options);
            services.AddAriHttpClient<IPlaybackAriActions, PlaybackAriActions>(options);
            services.AddAriHttpClient<ISoundAriActions, SoundAriActions>(options);
        }
    }
}