using AsteriskAriService.Models.Asterisk.Event;

namespace AsteriskAriService.Extensions
{
    public static class AriEventExtensions
    {
        public static Type GetEventModel(this AriEventType eventType)
        {
            return eventType switch
            {
                AriEventType.ApplicationMoveFailed => typeof(ApplicationMoveFailedEvent),
                AriEventType.ApplicationRegistered => typeof(ApplicationRegisteredEvent),
                AriEventType.ApplicationReplaced => typeof(ApplicationReplacedEvent),
                AriEventType.ApplicationUnregistered => typeof(ApplicationUnregisteredEvent),
                AriEventType.BridgeAttendedTransfer => typeof(BridgeAttendedTransferEvent),
                AriEventType.BridgeBlindTransfer => typeof(BridgeBlindTransferEvent),
                AriEventType.BridgeCreated => typeof(BridgeCreatedEvent),
                AriEventType.BridgeDestroyed => typeof(BridgeDestroyedEvent),
                AriEventType.BridgeMerged => typeof(BridgeMergedEvent),
                AriEventType.BridgeVideoSourceChanged => typeof(BridgeVideoSourceChangedEvent),
                AriEventType.CallBroadcast => typeof(CallBroadcastEvent),
                AriEventType.CallClaimed => typeof(CallClaimedEvent),
                AriEventType.ChannelCallerId => typeof(ChannelCallerIdEvent),
                AriEventType.ChannelConnectedLine => typeof(ChannelConnectedLineEvent),
                AriEventType.ChannelCreated => typeof(ChannelCreatedEvent),
                AriEventType.ChannelDestroyed => typeof(ChannelDestroyedEvent),
                AriEventType.ChannelDialplan => typeof(ChannelDialplanEvent),
                AriEventType.ChannelDtmfReceived => typeof(ChannelDtmfReceivedEvent),
                AriEventType.ChannelEnteredBridge => typeof(ChannelEnteredBridgeEvent),
                AriEventType.ChannelHangupRequest => typeof(ChannelHangupRequestEvent),
                AriEventType.ChannelHold => typeof(ChannelHoldEvent),
                AriEventType.ChannelLeftBridge => typeof(ChannelLeftBridgeEvent),
                AriEventType.ChannelStateChange => typeof(ChannelStateChangeEvent),
                AriEventType.ChannelTalkingFinished => typeof(ChannelTalkingFinishedEvent),
                AriEventType.ChannelTalkingStarted => typeof(ChannelTalkingStartedEvent),
                AriEventType.ChannelToneDetected => typeof(ChannelToneDetectedEvent),
                AriEventType.ChannelTransfer => typeof(ChannelTransferEvent),
                AriEventType.ChannelUnhold => typeof(ChannelUnholdEvent),   
                AriEventType.ChannelUserevent => typeof(ChannelUsereventEvent),
                AriEventType.ChannelVarset => typeof(ChannelVarsetEvent),
                AriEventType.ContactStatusChange => typeof(ContactStatusChangeEvent),
                AriEventType.DeviceStateChanged => typeof(DeviceStateChangedEvent),
                AriEventType.Dial => typeof(DialEvent),
                AriEventType.EndpointStateChange => typeof(EndpointStateChangeEvent),
                AriEventType.PeerStatusChange => typeof(PeerStatusChangeEvent),
                AriEventType.PlaybackContinuing => typeof(PlaybackContinuingEvent), 
                AriEventType.PlaybackFinished => typeof(PlaybackFinishedEvent),
                AriEventType.PlaybackStarted => typeof(PlaybackStartedEvent),
                AriEventType.RESTResponse => typeof(RESTResponseEvent),
                AriEventType.RecordingFailed => typeof(RecordingFailedEvent),
                AriEventType.RecordingFinished => typeof(RecordingFinishedEvent),
                AriEventType.RecordingStarted => typeof(RecordingStartedEvent),
                AriEventType.StasisEnd => typeof(StasisEndEvent),  
                AriEventType.StasisStart => typeof(StasisStartEvent),       
                AriEventType.TextMessageReceived => typeof(TextMessageReceivedEvent),
                _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null)
            };
        }
    }
}