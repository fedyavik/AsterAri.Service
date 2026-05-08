using System.Net.Http.Json;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Tools;

namespace AsteriskAriService.Actions.Playback
{
    public class PlaybackAriActions(HttpClient httpClient) : IPlaybackAriActions
    {
        public async Task<PlaybackModel> GetAsync(string playbackId)
        {
            var response = await httpClient.GetAsync($"playbacks/{playbackId}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("The playback cannot be found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<PlaybackModel>())!;
        }

        public async Task StopAsync(string playbackId)
        {
            var response = await httpClient.DeleteAsync($"playbacks/{playbackId}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("The playback cannot be found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task ControlAsync(string playbackId, string operation)
        {
            var pathWithQuery = new QueryBuilder($"playbacks/{playbackId}/control")
                .Add("operation", operation);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("The provided operation parameter was invalid", (int)response.StatusCode),
                    404 => new AsterAriException("The playback cannot be found", (int)response.StatusCode),
                    409 => new AsterAriException("The operation cannot be performed in the playback's current state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }
    }
}