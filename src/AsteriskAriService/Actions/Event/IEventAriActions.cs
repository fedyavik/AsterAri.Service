using AsteriskAriService.Models.Asterisk.Event;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Actions.Event
{
    public interface IEventAriActions
    {
        /// <summary>
        /// WebSocket connection for events.
        /// </summary>
        /// <param name="app">Applications to subscribe to.</param>
        /// <param name="subscribeAll">Subscribe to all Asterisk events.
        /// If provided, the applications listed will be subscribed to all events,
        /// effectively disabling the application specific subscriptions. Default is 'false'.</param>
        Task<MessageModel> EventWebsocketAsync(string app, bool subscribeAll = false);
        
        /// <summary>
        /// Generate a user event.
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="application">The name of the application that will receive this event</param>
        /// <param name="source">URI for event source
        /// (channel:{channelId}, bridge:{bridgeId}, endpoint:{tech}/{resource}, deviceState:{deviceName}).
        /// Allows comma separated values.</param>
        /// <param name="variables">The "variables" key in the body object holds custom key/value pairs to add to the user event. Ex. { "variables": { "key": "value" } }</param>
        /// <exception cref="AsterAriException">400 - Invalid even tsource URI or userevent data.</exception>
        /// <exception cref="AsterAriException">404 - Application does not exist.</exception>
        /// <exception cref="AsterAriException">422 - Event source not found.</exception>
        Task UserEventAsync(string eventName, string application, string? source = null, Dictionary<string, string>? variables = null);
        
        /// <summary>
        /// Claim a broadcast channel for this application.
        /// Atomically claims a channel that is in broadcast state. Only the first claim succeeds.
        /// </summary>
        /// <param name="channelId"> The ID of the channel to claim</param>
        /// <param name="application">The name of the application that will receive this event</param>
        /// <exception cref="AsterAriException">404 - Channel not found or not in broadcast state.</exception>
        /// <exception cref="AsterAriException">409 - Channel has already been claimed by another application.</exception>
        Task ClaimChannelAsync(string channelId, string application);
    }
}