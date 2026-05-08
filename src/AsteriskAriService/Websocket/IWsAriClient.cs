using AsteriskAriService.Models.Asterisk.Event;

namespace AsteriskAriService.Websocket
{
    public interface IWsAriClient
    {
        public IObservable<ApplicationMoveFailedEvent> OnApplicationMoveFailed { get; }
        public IObservable<ApplicationRegisteredEvent> OnApplicationRegistered { get; }
        public IObservable<ApplicationReplacedEvent> OnApplicationReplaced { get; } 
        public IObservable<ApplicationUnregisteredEvent> OnApplicationUnregistered { get; }
        public IObservable<BridgeAttendedTransferEvent> OnBridgeAttendedTransfer { get; }
        public IObservable<BridgeBlindTransferEvent> OnBridgeBlindTransfer { get; }
        public IObservable<BridgeCreatedEvent> OnBridgeCreated { get; }
        public IObservable<BridgeDestroyedEvent> OnBridgeDestroyed { get; }
        public IObservable<BridgeMergedEvent> OnBridgeMerged { get; }
        public IObservable<BridgeVideoSourceChangedEvent> OnBridgeVideoSourceChanged { get; }
        public IObservable<CallBroadcastEvent> OnCallBroadcast { get; }
        public IObservable<CallClaimedEvent> OnCallClaimed { get; }
        public IObservable<ChannelCallerIdEvent> OnChannelCallerId { get; }
        public IObservable<ChannelConnectedLineEvent> OnChannelConnectedLine { get; }
        public IObservable<ChannelCreatedEvent> OnChannelCreated { get; }
        public IObservable<ChannelDestroyedEvent> OnChannelDestroyed { get; }
        public IObservable<ChannelDialplanEvent> OnChannelDialplan { get; }
        public IObservable<ChannelDtmfReceivedEvent> OnChannelDtmfReceived { get; }
        public IObservable<ChannelEnteredBridgeEvent> OnChannelEnteredBridge { get; }
        public IObservable<ChannelHangupRequestEvent> OnChannelHangupRequest { get; }
        public IObservable<ChannelHoldEvent> OnChannelHold { get; }
        public IObservable<ChannelLeftBridgeEvent> OnChannelLeftBridge { get; }
        public IObservable<ChannelStateChangeEvent> OnChannelStateChange { get; }
        public IObservable<ChannelTalkingFinishedEvent> OnChannelTalkingFinished { get; }
        public IObservable<ChannelTalkingStartedEvent> OnChannelTalkingStarted { get; }
        public IObservable<ChannelToneDetectedEvent> OnChannelToneDetected { get; }
        public IObservable<ChannelTransferEvent> OnChannelTransfer { get; }
        public IObservable<ChannelUnholdEvent> OnChannelUnhold { get; }
        public IObservable<ChannelUsereventEvent> OnChannelUserevent { get; }
        public IObservable<ChannelVarsetEvent> OnChannelVarset { get; }
        public IObservable<ContactStatusChangeEvent> OnContactStatusChange { get; }
        public IObservable<DeviceStateChangedEvent> OnDeviceStateChanged { get; }
        public IObservable<DialEvent> OnDial { get; }
        public IObservable<EndpointStateChangeEvent> OnEndpointStateChange { get; }
        public IObservable<PeerStatusChangeEvent> OnPeerStatusChange { get; }
        public IObservable<PlaybackContinuingEvent> OnPlaybackContinuing { get; }
        public IObservable<PlaybackFinishedEvent> OnPlaybackFinished { get; }
        public IObservable<PlaybackStartedEvent> OnPlaybackStarted { get; }
        public IObservable<RESTResponseEvent> OnRestResponse { get; }
        public IObservable<RecordingFailedEvent> OnRecordingFailed { get; }
        public IObservable<RecordingFinishedEvent> OnRecordingFinished { get; }
        public IObservable<RecordingStartedEvent> OnRecordingStarted { get; }
        public IObservable<StasisEndEvent> OnStasisEnd { get; }
        public IObservable<StasisStartEvent> OnStasisStart { get; }
        public IObservable<TextMessageReceivedEvent> OnTextMessageReceived { get; }
    }
}