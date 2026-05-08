using System.Net.Http.Json;
using AsteriskAriService.Models.Asterisk.Event;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Tools;

namespace AsteriskAriService.Actions.Event
{
    public class EventAriActions(HttpClient httpClient) : IEventAriActions
    {
        public async Task<MessageModel> EventWebsocketAsync(string app, bool subscribeAll = false)
        {
            var pathWithQuery = new QueryBuilder("events")
                .Add("app", app)
                .Add("subscribeAll", subscribeAll);
            var response = await httpClient.GetAsync(pathWithQuery.ToString());
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<MessageModel>())!;
        }

        public async Task UserEventAsync(string eventName, string application, string? source = null, Dictionary<string, string>? variables = null)
        {
            var pathWithQuery = new QueryBuilder($"events/user/{eventName}")
                .Add("application", application)
                .Add("source", source);
            var data = new
            {
                variables
            };
            var response = await httpClient.PostAsJsonAsync(pathWithQuery.ToString(), data);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Invalid even tsource URI or userevent data.", (int)response.StatusCode),
                    404 => new AsterAriException("Application does not exist.", (int)response.StatusCode),
                    422 => new AsterAriException("Event source not found.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task ClaimChannelAsync(string channelId, string application)
        {
            var pathWithQuery = new QueryBuilder("events/claim")
                .Add("channelId", channelId)
                .Add("application", application);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Channel not found or not in broadcast state.", (int)response.StatusCode),
                    409 => new AsterAriException("Channel has already been claimed by another application.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }
    }
}