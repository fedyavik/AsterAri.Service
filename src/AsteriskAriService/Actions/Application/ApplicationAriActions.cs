using System.Net.Http.Json;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Tools;

namespace AsteriskAriService.Actions.Application
{
    public class ApplicationAriActions(
        HttpClient httpClient)
        : IApplicationAriActions
    {
        public async Task<List<ApplicationModel>> ListAsync()
        {
            var response = await httpClient.GetAsync("applications");
            return (await response.Content.ReadFromJsonAsync<List<ApplicationModel>>())!;
        }

        public async Task<ApplicationModel> GetAsync(string applicationName)
        {
            var response = await httpClient.GetAsync($"applications/{applicationName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Application does not exist.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<ApplicationModel>())!;
        }

        public async Task<ApplicationModel> SubscribeAsync(string applicationName, params string[] eventSource)
        {
            var pathWithQuery = new QueryBuilder($"applications/{applicationName}/subscription")
                .Add("eventSource", string.Join(",", eventSource));
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400  => new AsterAriException("Missing parameter.", (int)response.StatusCode),
                    404  => new AsterAriException("Application does not exist.", (int)response.StatusCode),
                    422  => new AsterAriException("Event source does not exist.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<ApplicationModel>())!;
        }

        public async Task<ApplicationModel> UnsubscribeAsync(string applicationName, params string[] eventSource)
        {
            var pathWithQuery = new QueryBuilder($"applications/{applicationName}/subscription")
                .Add("eventSource", string.Join(",", eventSource));
            var response = await httpClient.DeleteAsync(pathWithQuery.ToString());
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400  => new AsterAriException("Missing parameter.", (int)response.StatusCode),
                    404  => new AsterAriException("Application does not exist.", (int)response.StatusCode),
                    409  => new AsterAriException("Application not subscribed to event source.", (int)response.StatusCode),
                    422  => new AsterAriException("Event source does not exist.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<ApplicationModel>())!;
        }
        public async Task<ApplicationModel> FilterAsync(string applicationName, object? filter = null)
        {
            var response = await httpClient.PostAsJsonAsync($"applications/{applicationName}/eventFilter", filter);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400  => new AsterAriException("Bad request.", (int)response.StatusCode),
                    404  => new AsterAriException("Application does not exist.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<ApplicationModel>())!;
        }
    }
}