using System.Buffers;
using System.Net.WebSockets;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Threading.Channels;
using AsteriskAriService.Extensions;
using AsteriskAriService.Models;
using AsteriskAriService.Models.Asterisk.Event;
using AsteriskAriService.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AsteriskAriService.Websocket
{
    public class WsClientAri : IWsAriClient
    {
        #region Subjects
        private Subject<ApplicationMoveFailedEvent> ApplicationMoveFailed { get; set; } = new();
        public IObservable<ApplicationMoveFailedEvent> OnApplicationMoveFailed => ApplicationMoveFailed;

        private Subject<ApplicationRegisteredEvent> ApplicationRegistered { get; set; } = new();
        public IObservable<ApplicationRegisteredEvent> OnApplicationRegistered => ApplicationRegistered;
        
        private Subject<ApplicationReplacedEvent> ApplicationReplaced { get; set; } = new();
        public IObservable<ApplicationReplacedEvent> OnApplicationReplaced => ApplicationReplaced;   
        
        private Subject<ApplicationUnregisteredEvent> ApplicationUnregistered { get; set; } = new();
        public IObservable<ApplicationUnregisteredEvent> OnApplicationUnregistered => ApplicationUnregistered;

        private Subject<BridgeAttendedTransferEvent> BridgeAttendedTransfer { get; set; } = new();
        public IObservable<BridgeAttendedTransferEvent> OnBridgeAttendedTransfer => BridgeAttendedTransfer;
        
        private Subject<BridgeBlindTransferEvent> BridgeBlindTransfer { get; set; } = new();
        public IObservable<BridgeBlindTransferEvent> OnBridgeBlindTransfer => BridgeBlindTransfer;
        
        private Subject<BridgeCreatedEvent> BridgeCreated { get; set; } = new();
        public IObservable<BridgeCreatedEvent> OnBridgeCreated => BridgeCreated;
        
        private Subject<BridgeDestroyedEvent> BridgeDestroyed { get; set; } = new();
        public IObservable<BridgeDestroyedEvent> OnBridgeDestroyed => BridgeDestroyed;
        
        private Subject<BridgeMergedEvent> BridgeMerged { get; set; } = new();
        public IObservable<BridgeMergedEvent> OnBridgeMerged => BridgeMerged;
        
        private Subject<BridgeVideoSourceChangedEvent> BridgeVideoSourceChanged { get; set; } = new();
        public IObservable<BridgeVideoSourceChangedEvent> OnBridgeVideoSourceChanged => BridgeVideoSourceChanged;
        
        private Subject<CallBroadcastEvent> CallBroadcast { get; set; } = new();
        public IObservable<CallBroadcastEvent> OnCallBroadcast => CallBroadcast;
        
        private Subject<CallClaimedEvent> CallClaimed { get; set; } = new();
        public IObservable<CallClaimedEvent> OnCallClaimed => CallClaimed;
        
        private Subject<ChannelCallerIdEvent> ChannelCallerId { get; set; } = new();
        public IObservable<ChannelCallerIdEvent> OnChannelCallerId => ChannelCallerId;
        
        private Subject<ChannelConnectedLineEvent> ChannelConnectedLine { get; set; } = new();
        public IObservable<ChannelConnectedLineEvent> OnChannelConnectedLine => ChannelConnectedLine;
        
        private Subject<ChannelCreatedEvent> ChannelCreated { get; set; } = new();
        public IObservable<ChannelCreatedEvent> OnChannelCreated => ChannelCreated;
        
        private Subject<ChannelDestroyedEvent> ChannelDestroyed { get; set; } = new();
        public IObservable<ChannelDestroyedEvent> OnChannelDestroyed => ChannelDestroyed;
        
        private Subject<ChannelDialplanEvent> ChannelDialplan { get; set; } = new();
        public IObservable<ChannelDialplanEvent> OnChannelDialplan => ChannelDialplan;
        
        private Subject<ChannelDtmfReceivedEvent> ChannelDtmfReceived { get; set; } = new();
        public IObservable<ChannelDtmfReceivedEvent> OnChannelDtmfReceived => ChannelDtmfReceived;
        
        private Subject<ChannelEnteredBridgeEvent> ChannelEnteredBridge { get; set; } = new();
        public IObservable<ChannelEnteredBridgeEvent> OnChannelEnteredBridge => ChannelEnteredBridge;
        
        private Subject<ChannelHangupRequestEvent> ChannelHangupRequest { get; set; } = new();
        public IObservable<ChannelHangupRequestEvent> OnChannelHangupRequest => ChannelHangupRequest;
        
        private Subject<ChannelHoldEvent> ChannelHold { get; set; } = new();
        public IObservable<ChannelHoldEvent> OnChannelHold => ChannelHold;
        
        private Subject<ChannelLeftBridgeEvent> ChannelLeftBridge { get; set; } = new();
        public IObservable<ChannelLeftBridgeEvent> OnChannelLeftBridge => ChannelLeftBridge;
        
        private Subject<ChannelStateChangeEvent> ChannelStateChange { get; set; } = new();
        public IObservable<ChannelStateChangeEvent> OnChannelStateChange => ChannelStateChange;
        
        private Subject<ChannelTalkingFinishedEvent> ChannelTalkingFinished { get; set; } = new();
        public IObservable<ChannelTalkingFinishedEvent> OnChannelTalkingFinished => ChannelTalkingFinished;
        
        private Subject<ChannelTalkingStartedEvent> ChannelTalkingStarted { get; set; } = new();
        public IObservable<ChannelTalkingStartedEvent> OnChannelTalkingStarted => ChannelTalkingStarted;
        
        private Subject<ChannelToneDetectedEvent> ChannelToneDetected { get; set; } = new();
        public IObservable<ChannelToneDetectedEvent> OnChannelToneDetected => ChannelToneDetected;
        
        private Subject<ChannelTransferEvent> ChannelTransfer { get; set; } = new();
        public IObservable<ChannelTransferEvent> OnChannelTransfer => ChannelTransfer;
        
        private Subject<ChannelUnholdEvent> ChannelUnhold { get; set; } = new();
        public IObservable<ChannelUnholdEvent> OnChannelUnhold => ChannelUnhold;
        
        private Subject<ChannelUsereventEvent> ChannelUserevent { get; set; } = new();
        public IObservable<ChannelUsereventEvent> OnChannelUserevent => ChannelUserevent;
        
        private Subject<ChannelVarsetEvent> ChannelVarset { get; set; } = new();
        public IObservable<ChannelVarsetEvent> OnChannelVarset => ChannelVarset;
        
        private Subject<ContactStatusChangeEvent> ContactStatusChange { get; set; } = new();
        public IObservable<ContactStatusChangeEvent> OnContactStatusChange => ContactStatusChange;
        
        private Subject<DeviceStateChangedEvent> DeviceStateChanged { get; set; } = new();
        public IObservable<DeviceStateChangedEvent> OnDeviceStateChanged => DeviceStateChanged;
        
        private Subject<DialEvent> Dial { get; set; } = new();
        public IObservable<DialEvent> OnDial => Dial;
        
        private Subject<EndpointStateChangeEvent> EndpointStateChange { get; set; } = new();
        public IObservable<EndpointStateChangeEvent> OnEndpointStateChange => EndpointStateChange;
        
        private Subject<PeerStatusChangeEvent> PeerStatusChange { get; set; } = new();
        public IObservable<PeerStatusChangeEvent> OnPeerStatusChange => PeerStatusChange;
        
        private Subject<PlaybackContinuingEvent> PlaybackContinuing { get; set; } = new();
        public IObservable<PlaybackContinuingEvent> OnPlaybackContinuing => PlaybackContinuing;
        
        private Subject<PlaybackFinishedEvent> PlaybackFinished { get; set; } = new();
        public IObservable<PlaybackFinishedEvent> OnPlaybackFinished => PlaybackFinished;
        
        private Subject<PlaybackStartedEvent> PlaybackStarted { get; set; } = new();
        public IObservable<PlaybackStartedEvent> OnPlaybackStarted => PlaybackStarted;
        
        private Subject<RESTResponseEvent> RestResponse { get; set; } = new();
        public IObservable<RESTResponseEvent> OnRestResponse => RestResponse;
        
        private Subject<RecordingFailedEvent> RecordingFailed { get; set; } = new();
        public IObservable<RecordingFailedEvent> OnRecordingFailed => RecordingFailed;
        
        private Subject<RecordingFinishedEvent> RecordingFinished { get; set; } = new();
        public IObservable<RecordingFinishedEvent> OnRecordingFinished => RecordingFinished;
        
        private Subject<RecordingStartedEvent> RecordingStarted { get; set; } = new();
        public IObservable<RecordingStartedEvent> OnRecordingStarted => RecordingStarted;
        
        private Subject<StasisEndEvent> StasisEnd { get; set; } = new();
        public IObservable<StasisEndEvent> OnStasisEnd => StasisEnd;
        
        private Subject<StasisStartEvent> StasisStart { get; set; } = new();
        public IObservable<StasisStartEvent> OnStasisStart => StasisStart;
        
        private Subject<TextMessageReceivedEvent> TextMessageReceived { get; set; } = new();
        public IObservable<TextMessageReceivedEvent> OnTextMessageReceived => TextMessageReceived;
        #endregion
     
        private readonly AriConnectOptions _options;
        private readonly ILogger _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly Channel<EventModel> _channel;
        private readonly CancellationTokenSource _cts = new();
        
        private static readonly Dictionary<string, AriEventType> AriEventMap = new(StringComparer.OrdinalIgnoreCase)
            {
                ["ApplicationMoveFailed"] = AriEventType.ApplicationMoveFailed,
                ["ApplicationRegistered"] = AriEventType.ApplicationRegistered,
                ["ApplicationReplaced"] = AriEventType.ApplicationReplaced,
                ["ApplicationUnregistered"] = AriEventType.ApplicationUnregistered,
                ["BridgeAttendedTransfer"] = AriEventType.BridgeAttendedTransfer,
                ["BridgeBlindTransfer"] = AriEventType.BridgeBlindTransfer,
                ["BridgeCreated"] = AriEventType.BridgeCreated,
                ["BridgeDestroyed"] = AriEventType.BridgeDestroyed,
                ["BridgeMerged"] = AriEventType.BridgeMerged,
                ["BridgeVideoSourceChanged"] = AriEventType.BridgeVideoSourceChanged,
                ["CallBroadcast"] = AriEventType.CallBroadcast,
                ["CallClaimed"] = AriEventType.CallClaimed,
                ["ChannelCallerId"] = AriEventType.ChannelCallerId,
                ["ChannelConnectedLine"] = AriEventType.ChannelConnectedLine,
                ["ChannelCreated"] = AriEventType.ChannelCreated,
                ["ChannelDestroyed"] = AriEventType.ChannelDestroyed,
                ["ChannelDialplan"] = AriEventType.ChannelDialplan,
                ["ChannelDtmfReceived"] = AriEventType.ChannelDtmfReceived,
                ["ChannelEnteredBridge"] = AriEventType.ChannelEnteredBridge,
                ["ChannelHangupRequest"] = AriEventType.ChannelHangupRequest,
                ["ChannelHold"] = AriEventType.ChannelHold,
                ["ChannelLeftBridge"] = AriEventType.ChannelLeftBridge,
                ["ChannelStateChange"] = AriEventType.ChannelStateChange,
                ["ChannelTalkingFinished"] = AriEventType.ChannelTalkingFinished,
                ["ChannelTalkingStarted"] = AriEventType.ChannelTalkingStarted,
                ["ChannelToneDetected"] = AriEventType.ChannelToneDetected,
                ["ChannelTransfer"] = AriEventType.ChannelTransfer,
                ["ChannelUnhold"] = AriEventType.ChannelUnhold,
                ["ChannelUserevent"] = AriEventType.ChannelUserevent,
                ["ChannelVarset"] = AriEventType.ChannelVarset,
                ["ContactStatusChange"] = AriEventType.ContactStatusChange,
                ["DeviceStateChanged"] = AriEventType.DeviceStateChanged,
                ["Dial"] = AriEventType.Dial,
                ["EndpointStateChange"] = AriEventType.EndpointStateChange,
                ["PeerStatusChange"] = AriEventType.PeerStatusChange,
                ["PlaybackContinuing"] = AriEventType.PlaybackContinuing,
                ["PlaybackFinished"] = AriEventType.PlaybackFinished,
                ["PlaybackStarted"] = AriEventType.PlaybackStarted,
                ["RESTResponse"] = AriEventType.RESTResponse,
                ["RecordingFailed"] = AriEventType.RecordingFailed,
                ["RecordingFinished"] = AriEventType.RecordingFinished,
                ["RecordingStarted"] = AriEventType.RecordingStarted,
                ["StasisEnd"] = AriEventType.StasisEnd,
                ["StasisStart"] = AriEventType.StasisStart,
                ["TextMessageReceived"] = AriEventType.TextMessageReceived,
            };
        
        public WsClientAri(IOptions<ServerAriOptions> options, ILogger<WsClientAri> logger)
        {
            _logger = logger;
            _options = options.Value.ConnectOptions;
            _jsonOptions = GetJsonOptions();
            _channel = Channel.CreateUnbounded<EventModel>(new UnboundedChannelOptions()
            {
                SingleReader = true,
                SingleWriter = true,
                AllowSynchronousContinuations = false
            });
            _ = Connect();
            _ = EventConsumer();
        }

        private async Task EventConsumer()
        {
            await foreach (var evt in _channel.Reader.ReadAllAsync(_cts.Token))
                InvokeEvent(evt.EventType, evt);
        }
        private async Task Connect()
        {
            var scheme = _options.UseSsl ? "wss://" : "ws://";
            var uri = $"{scheme}{_options.Hostname}:{_options.Port}/ari/events?" +
                      $"app={_options.AppName}&subscribeAll={true}&api_key={_options.UserName}:{_options.Password}";
            var delay = TimeSpan.FromSeconds(1);
            var maxDelay = TimeSpan.FromSeconds(30);

            while (!_cts.IsCancellationRequested)
            {
                using var ws = new ClientWebSocket();
                ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
                try
                {
                    await ws.ConnectAsync(new Uri(uri), _cts.Token);
                    _logger.LogWarning("Ari websocket connected");
                    delay = TimeSpan.FromSeconds(1);
                    await ReceiveLoop(ws, _cts.Token);
                }
                catch (WebSocketException)
                {
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    _logger.LogWarning("Ari websocket error: {Message}", e.Message);
                }

                await Task.Delay(delay, _cts.Token);
                delay = TimeSpan.FromSeconds(
                    Math.Min(delay.TotalSeconds * 2, maxDelay.TotalSeconds)
                );
            }
        }
        private async Task ReceiveLoop(ClientWebSocket ws, CancellationToken ct)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(4096);
            using var ms = new MemoryStream();
            try
            {
                while (ws.State == WebSocketState.Open && !ct.IsCancellationRequested)
                {
                    ms.SetLength(0);
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await ws.ReceiveAsync(buffer, ct);
                        if (result.MessageType == WebSocketMessageType.Close)
                            return;
                        ms.Write(buffer, 0, result.Count);
                    } while (!result.EndOfMessage);
                    ms.Position = 0;
                    ReadMessage(ms);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        private void ReadMessage(MemoryStream ms)
        {
            var jsonSpan = ms.GetBuffer().AsSpan(0, (int)ms.Length);
            var reader = new Utf8JsonReader(jsonSpan);
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName ||
                    !reader.ValueTextEquals("type")) continue;
                
                reader.Read();
                var type = reader.GetString();
                if (!AriEventMap.TryGetValue(type!, out var eventType))
                {
                    _logger.LogWarning("Ari websocket unknown event: {Event}", type);
                    return;
                }
                if (!HasEventSubscribers(eventType))
                    return;
                var modelType = eventType.GetEventModel();
                var eventModel = (EventModel)JsonSerializer.Deserialize(jsonSpan,modelType,_jsonOptions)!;
                eventModel.EventType = eventType;
                _channel.Writer.TryWrite(eventModel);
                break;
            }
        }
        private JsonSerializerOptions GetJsonOptions()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AsteriskDateTimeConverter());
            options.PropertyNameCaseInsensitive = true;
            return options;
        }
        private bool HasEventSubscribers(AriEventType eventType)
        {
            return eventType switch
            {
                AriEventType.ApplicationMoveFailed => ApplicationMoveFailed.HasObservers,
                AriEventType.ApplicationRegistered => ApplicationRegistered.HasObservers,
                AriEventType.ApplicationReplaced => ApplicationReplaced.HasObservers,
                AriEventType.ApplicationUnregistered => ApplicationUnregistered.HasObservers,
                AriEventType.BridgeAttendedTransfer => BridgeAttendedTransfer.HasObservers,
                AriEventType.BridgeBlindTransfer => BridgeBlindTransfer.HasObservers,
                AriEventType.BridgeCreated =>  BridgeCreated.HasObservers,
                AriEventType.BridgeDestroyed => BridgeDestroyed.HasObservers,
                AriEventType.BridgeMerged => BridgeMerged.HasObservers,
                AriEventType.BridgeVideoSourceChanged => BridgeVideoSourceChanged.HasObservers,
                AriEventType.CallBroadcast => CallBroadcast.HasObservers,
                AriEventType.CallClaimed => CallClaimed.HasObservers,
                AriEventType.ChannelCallerId => ChannelCallerId.HasObservers,
                AriEventType.ChannelConnectedLine => ChannelConnectedLine.HasObservers,
                AriEventType.ChannelCreated => ChannelCreated.HasObservers,
                AriEventType.ChannelDestroyed => ChannelDestroyed.HasObservers,
                AriEventType.ChannelDialplan => ChannelDialplan.HasObservers,
                AriEventType.ChannelDtmfReceived => ChannelDtmfReceived.HasObservers,
                AriEventType.ChannelEnteredBridge => ChannelEnteredBridge.HasObservers,
                AriEventType.ChannelHangupRequest => ChannelHangupRequest.HasObservers,
                AriEventType.ChannelHold => ChannelHold.HasObservers,
                AriEventType.ChannelLeftBridge => ChannelLeftBridge.HasObservers,
                AriEventType.ChannelStateChange => ChannelStateChange.HasObservers,
                AriEventType.ChannelTalkingFinished => ChannelTalkingFinished.HasObservers,
                AriEventType.ChannelTalkingStarted => ChannelTalkingStarted.HasObservers,
                AriEventType.ChannelToneDetected => ChannelToneDetected.HasObservers,
                AriEventType.ChannelTransfer => ChannelTransfer.HasObservers,
                AriEventType.ChannelUnhold => ChannelUnhold.HasObservers,
                AriEventType.ChannelUserevent => ChannelUserevent.HasObservers,
                AriEventType.ChannelVarset => ChannelVarset.HasObservers,
                AriEventType.ContactStatusChange => ContactStatusChange.HasObservers,
                AriEventType.DeviceStateChanged => DeviceStateChanged.HasObservers,
                AriEventType.Dial => Dial.HasObservers,
                AriEventType.EndpointStateChange => EndpointStateChange.HasObservers,
                AriEventType.PeerStatusChange => PeerStatusChange.HasObservers,
                AriEventType.PlaybackContinuing => PlaybackContinuing.HasObservers,
                AriEventType.PlaybackFinished => PlaybackFinished.HasObservers,
                AriEventType.PlaybackStarted => PlaybackStarted.HasObservers,
                AriEventType.RESTResponse => RestResponse.HasObservers,
                AriEventType.RecordingFailed => RecordingFailed.HasObservers,
                AriEventType.RecordingFinished => RecordingFinished.HasObservers,
                AriEventType.RecordingStarted => RecordingStarted.HasObservers,
                AriEventType.StasisEnd => StasisEnd.HasObservers,
                AriEventType.StasisStart => StasisStart.HasObservers,
                AriEventType.TextMessageReceived => TextMessageReceived.HasObservers,
                _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null)
            };
        }
        private void InvokeEvent(AriEventType eventType, object model)
        {
            switch (eventType)
            {
                case AriEventType.ApplicationMoveFailed:
                    ApplicationMoveFailed.OnNext((ApplicationMoveFailedEvent)model);
                    break;
                case AriEventType.ApplicationRegistered:
                    ApplicationRegistered.OnNext((ApplicationRegisteredEvent)model);
                    break;
                case AriEventType.ApplicationReplaced:
                    ApplicationReplaced.OnNext((ApplicationReplacedEvent)model);
                    break;
                case AriEventType.ApplicationUnregistered:
                    ApplicationUnregistered.OnNext((ApplicationUnregisteredEvent)model);
                    break;
                case AriEventType.BridgeAttendedTransfer:
                    BridgeAttendedTransfer.OnNext((BridgeAttendedTransferEvent)model);
                    break;
                case AriEventType.BridgeBlindTransfer:
                    BridgeBlindTransfer.OnNext((BridgeBlindTransferEvent)model);
                    break;
                case AriEventType.BridgeCreated:
                    BridgeCreated.OnNext((BridgeCreatedEvent)model);
                    break;
                case AriEventType.BridgeDestroyed:
                    BridgeDestroyed.OnNext((BridgeDestroyedEvent)model);
                    break;
                case AriEventType.BridgeMerged:
                    BridgeMerged.OnNext((BridgeMergedEvent)model);
                    break;
                case AriEventType.BridgeVideoSourceChanged:
                    BridgeVideoSourceChanged.OnNext((BridgeVideoSourceChangedEvent)model);
                    break;
                case AriEventType.CallBroadcast:
                    CallBroadcast.OnNext((CallBroadcastEvent)model);
                    break;
                case AriEventType.CallClaimed:
                    CallClaimed.OnNext((CallClaimedEvent)model);
                    break;
                case AriEventType.ChannelCallerId:
                    ChannelCallerId.OnNext((ChannelCallerIdEvent)model);
                    break;
                case AriEventType.ChannelConnectedLine:
                    ChannelConnectedLine.OnNext((ChannelConnectedLineEvent)model);
                    break;
                case AriEventType.ChannelCreated:
                    ChannelCreated.OnNext((ChannelCreatedEvent)model);
                    break;
                case AriEventType.ChannelDestroyed:
                    ChannelDestroyed.OnNext((ChannelDestroyedEvent)model);
                    break;
                case AriEventType.ChannelDialplan:
                    ChannelDialplan.OnNext((ChannelDialplanEvent)model);
                    break;
                case AriEventType.ChannelDtmfReceived:
                    ChannelDtmfReceived.OnNext((ChannelDtmfReceivedEvent)model);
                    break;
                case AriEventType.ChannelEnteredBridge:
                    ChannelEnteredBridge.OnNext((ChannelEnteredBridgeEvent)model);
                    break;
                case AriEventType.ChannelHangupRequest:
                    ChannelHangupRequest.OnNext((ChannelHangupRequestEvent)model);
                    break;
                case AriEventType.ChannelHold:
                    ChannelHold.OnNext((ChannelHoldEvent)model);
                    break;
                case AriEventType.ChannelLeftBridge:
                    ChannelLeftBridge.OnNext((ChannelLeftBridgeEvent)model);
                    break;
                case AriEventType.ChannelStateChange:
                    ChannelStateChange.OnNext((ChannelStateChangeEvent)model);
                    break;
                case AriEventType.ChannelTalkingFinished:
                    ChannelTalkingFinished.OnNext((ChannelTalkingFinishedEvent)model);
                    break;
                case AriEventType.ChannelTalkingStarted:
                    ChannelTalkingStarted.OnNext((ChannelTalkingStartedEvent)model);
                    break;
                case AriEventType.ChannelToneDetected:
                    ChannelToneDetected.OnNext((ChannelToneDetectedEvent)model);
                    break;
                case AriEventType.ChannelTransfer:
                    ChannelTransfer.OnNext((ChannelTransferEvent)model);
                    break;
                case AriEventType.ChannelUnhold:
                    ChannelUnhold.OnNext((ChannelUnholdEvent)model);
                    break;
                case AriEventType.ChannelUserevent:
                    ChannelUserevent.OnNext((ChannelUsereventEvent)model);
                    break;
                case AriEventType.ChannelVarset:
                    ChannelVarset.OnNext((ChannelVarsetEvent)model);
                    break;
                case AriEventType.ContactStatusChange:
                    ContactStatusChange.OnNext((ContactStatusChangeEvent)model);
                    break;
                case AriEventType.DeviceStateChanged:
                    DeviceStateChanged.OnNext((DeviceStateChangedEvent)model);
                    break;
                case AriEventType.Dial:
                    Dial.OnNext((DialEvent)model);
                    break;
                case AriEventType.EndpointStateChange:
                    EndpointStateChange.OnNext((EndpointStateChangeEvent)model);
                    break;
                case AriEventType.PeerStatusChange:
                    PeerStatusChange.OnNext((PeerStatusChangeEvent)model);
                    break;
                case AriEventType.PlaybackContinuing:
                    PlaybackContinuing.OnNext((PlaybackContinuingEvent)model);
                    break;
                case AriEventType.PlaybackFinished:
                    PlaybackFinished.OnNext((PlaybackFinishedEvent)model);
                    break;
                case AriEventType.PlaybackStarted:
                    PlaybackStarted.OnNext((PlaybackStartedEvent)model);
                    break;
                case AriEventType.RESTResponse:
                    RestResponse.OnNext((RESTResponseEvent)model);
                    break;
                case AriEventType.RecordingFailed:
                    RecordingFailed.OnNext((RecordingFailedEvent)model);
                    break;
                case AriEventType.RecordingFinished:
                    RecordingFinished.OnNext((RecordingFinishedEvent)model);
                    break;
                case AriEventType.RecordingStarted:
                    RecordingStarted.OnNext((RecordingStartedEvent)model);
                    break;
                case AriEventType.StasisEnd:
                    StasisEnd.OnNext((StasisEndEvent)model);
                    break;
                case AriEventType.StasisStart:
                    StasisStart.OnNext((StasisStartEvent)model);
                    break;
                case AriEventType.TextMessageReceived:
                    TextMessageReceived.OnNext((TextMessageReceivedEvent)model);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }
        }
    }
}