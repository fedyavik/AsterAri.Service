using System.Net.Http.Json;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Tools;

namespace AsteriskAriService.Actions.Sounds
{
    public class SoundAriActions(HttpClient httpClient) : ISoundAriActions
    {
        public async Task<List<SoundModel>> ListAsync(string? lang = null, string? format = null)
        {
            var pathWithQuery = new QueryBuilder("sounds")
                .Add("lang", lang)
                .Add("format", format);
            var response = await httpClient.GetAsync(pathWithQuery.ToString());
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<List<SoundModel>>())!;
        }

        public async Task<SoundModel> GetAsync(string soundId)
        {
            var response = await httpClient.GetAsync($"sounds/{soundId}");
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<SoundModel>())!;
        }
    }
}