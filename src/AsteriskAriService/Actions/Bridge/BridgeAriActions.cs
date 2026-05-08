using System.Net.Http.Json;
using System.Text.Json;
using AsteriskAriService.Actions.Channel;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Tools;

namespace AsteriskAriService.Actions.Bridge
{
    public class BridgeAriActions(
        HttpClient httpClient,
        IChannelAriActions channelAriActions
        ): IBridgeAriActions
    {
        public async Task<List<BridgeModel>> ListAsync()
        {
            var response = await httpClient.GetAsync($"bridges");
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<List<BridgeModel>>(GetJsonOptions()))!;
        }

        public async Task<BridgeModel> CreateAsync(string? type = null, string? bridgeId = null, string? name = null)
        {
            var pathWithQuery = new QueryBuilder($"bridges")
                .Add("type", type)
                .Add("bridgeId", bridgeId)
                .Add("name", name);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    409 => new AsterAriException("Bridge with the same bridgeId already exists", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<BridgeModel>(GetJsonOptions()))!;
        }

        public async Task<BridgeModel> GetAsync(string bridgeId)
        {
            var response = await httpClient.GetAsync($"bridges/{bridgeId}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Bridge not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<BridgeModel>(GetJsonOptions()))!;
        }

        public async Task HangupConnectedChannelsAsync(string bridgeId)
        {
            var bridgeDetail = await GetAsync(bridgeId);
            List<Task> hangupTasks = [];
            foreach (var channelId in bridgeDetail.Channels)
                hangupTasks.Add(channelAriActions.TryHangupAsync(channelId));
            await Task.WhenAll(hangupTasks);
        }

        public async Task StartMohAsync(string bridgeId, string? mohClass = null)
        {
            var pathWithQuery = new QueryBuilder($"bridges/{bridgeId}/moh")
                .Add("mohClass", mohClass);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Bridge not found", (int)response.StatusCode),
                    409 => new AsterAriException("Bridge not in Stasis application", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }
        public async Task StopMohAsync(string bridgeId)
        {
            var response = await httpClient.DeleteAsync($"bridges/{bridgeId}/moh");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Bridge not found", (int)response.StatusCode),
                    409 => new AsterAriException("Bridge not in Stasis application", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task AddChannelAsync(string bridgeId, string? role = null, bool absorbDtmf = false, bool mute = false,
            bool inhibitConnectedLineUpdates = true, params string[] channelsIds)
        {
            var pathWithQuery = new QueryBuilder($"bridges/{bridgeId}/addChannel")
                .Add("channel", string.Join(",", channelsIds))
                .Add("role", role)
                .Add("absorbDtmf", absorbDtmf)
                .Add("mute", mute)
                .Add("inhibitConnectedLineUpdates", inhibitConnectedLineUpdates);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    404 => new AsterAriException("Bridge not found", (int)response.StatusCode),
                    409 => new AsterAriException("Bridge not in Stasis application", (int)response.StatusCode),
                    422 => new AsterAriException("Channel not in Stasis application", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task TryAddChannelAsync(string bridgeId, string? role = null, bool absorbDtmf = false, bool mute = false,
            bool inhibitConnectedLineUpdates = true, params string[] channelsIds)
        {
            try
            {
                await AddChannelAsync(bridgeId, role, absorbDtmf, mute, inhibitConnectedLineUpdates, channelsIds);
            }
            catch (Exception)
            {
                //ignored
            }
        }

        public async Task RemoveChannelAsync(string bridgeId, params string[] channelsIds)
        {
            var pathWithQuery = new QueryBuilder($"bridges/{bridgeId}/removeChannel")
                .Add("channel", string.Join(",", channelsIds));
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    404 => new AsterAriException("Bridge not found", (int)response.StatusCode),
                    409 => new AsterAriException("Bridge not in Stasis application", (int)response.StatusCode),
                    422 => new AsterAriException("Channel not in Stasis application", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task TryRemoveChannelAsync(string bridgeId, params string[] channelsIds)
        {
            try
            {
                await RemoveChannelAsync(bridgeId, channelsIds);
            }
            catch (Exception)
            {
                //ignored
            }
        }

        public async Task DestroyAsync(string bridgeId)
        {
            var response = await httpClient.DeleteAsync($"bridges/{bridgeId}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Bridge not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task TryDestroyAsync(string bridgeId)
        {
            try
            {
                await DestroyAsync(bridgeId);
            }
            catch (Exception)
            {
                //ignored
            }
        }

        public async Task<LiveRecordingModel> RecordAsync(string bridgeId, string name, string format, int? maxDurationSeconds = null,
            int? maxSilenceSeconds = null, string? ifExists = null, bool? beep = null, string? terminateOn = null)
        {
            var pathWithQuery = new QueryBuilder($"bridges/{bridgeId}/record")
                .Add("name", name)
                .Add("format", format)
                .Add("maxDurationSeconds", maxDurationSeconds)
                .Add("maxSilenceSeconds", maxSilenceSeconds)
                .Add("ifExists", ifExists)
                .Add("beep", beep)
                .Add("terminateOn", terminateOn);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Invalid parameters", (int)response.StatusCode),
                    404 => new AsterAriException("Bridge not found", (int)response.StatusCode),
                    409 => new AsterAriException("Bridge is not in a Stasis application; " +
                                                 "A recording with the same name already exists on the system " +
                                                 "and can not be overwritten because it is in progress or ifExists=fail", (int)response.StatusCode),
                    422 => new AsterAriException("The format specified is unknown on this system", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<LiveRecordingModel>(GetJsonOptions()))!;
        }
        private JsonSerializerOptions GetJsonOptions()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AsteriskDateTimeConverter());
            options.PropertyNameCaseInsensitive = true;
            return options;
        }
    }
}