using System.Net.Http.Json;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Tools;

namespace AsteriskAriService.Actions.DeviceStates
{
    public class DeviceStatesAriActions(HttpClient httpClient):IDeviceStatesAriActions
    {
        public async Task<List<DeviceStateModel>> ListAsync()
        {
            var response = await httpClient.GetAsync("deviceStates");
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<List<DeviceStateModel>>())!;
        }

        public async Task<DeviceStateModel> GetAsync(string deviceName)
        {
            var response = await httpClient.GetAsync($"deviceStates/{deviceName}");
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<DeviceStateModel>())!;
        }

        public async Task UpdateAsync(string deviceName, string deviceState)
        {
            var pathWithQuery = new QueryBuilder($"deviceStates/{deviceName}")
                .Add("deviceState", deviceState);
            var response = await httpClient.PutAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Device name is missing", (int)response.StatusCode),
                    409 => new AsterAriException("Uncontrolled device specified", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task DeleteAsync(string deviceName)
        {
            var response = await httpClient.DeleteAsync($"deviceStates/{deviceName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Device name is missing", (int)response.StatusCode),
                    409 => new AsterAriException("Uncontrolled device specified", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }
    }
}