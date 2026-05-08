using System.Net.Http.Json;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Tools;

namespace AsteriskAriService.Actions.Endpoint
{
    public class EndpointAriActions(HttpClient httpClient) : IEndpointAriActions
    {
        public async Task<List<EndpointModel>> ListAsync()
        {
            var response = await httpClient.GetAsync($"endpoints");
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<List<EndpointModel>>())!;
        }

        public async Task SendMessageAsync(string to, string from, string? body = null, Dictionary<string, string>? variables = null)
        {
            var pathWithQuery = new QueryBuilder("endpoints/sendMessage")
                .Add("to", to)
                .Add("from", from)
                .Add("body", body);
            var data = new
            {
                variables
            };
            var response = await httpClient.PutAsJsonAsync(pathWithQuery.ToString(), data);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Invalid parameters for sending a message.", (int)response.StatusCode),
                    404 => new AsterAriException("Endpoint not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task ReferAsync(string to, string from, string referTo, bool toSelf, Dictionary<string, string>? variables = null)
        {
            var pathWithQuery = new QueryBuilder("endpoints/refer")
                .Add("to", to)
                .Add("from", from)
                .Add("refer_to", referTo)
                .Add("to_self", toSelf);
            var data = new
            {
                variables
            };
            var response = await httpClient.PostAsJsonAsync(pathWithQuery.ToString(), data);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Invalid parameters for referring.", (int)response.StatusCode),
                    404 => new AsterAriException("Endpoint not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task<List<EndpointModel>> ListByTechAsync(string tech)
        {
            var response = await httpClient.GetAsync($"endpoints/{tech}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Endpoint not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<List<EndpointModel>>())!;
        }

        public async Task<EndpointModel> GetAsync(string tech, string resource)
        {
            var response = await httpClient.GetAsync($"endpoints/{tech}/{resource}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Invalid parameters for sending a message", (int)response.StatusCode),
                    404 => new AsterAriException("Endpoint not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<EndpointModel>())!;
        }

        public async Task SendMessageToEndpointAsync(string tech, string resource, string from, string? body = null,
            Dictionary<string, string>? variables = null)
        {
            var pathWithQuery = new QueryBuilder($"endpoints/{tech}/{resource}/sendMessage")
                .Add("from", from)
                .Add("body", body);
            var data = new
            {
                variables
            };
            var response = await httpClient.PutAsJsonAsync(pathWithQuery.ToString(), data);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Invalid parameters for sending a message.", (int)response.StatusCode),
                    404 => new AsterAriException("Endpoint not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task ReferToEndpointAsync(string tech, string resource, string from, 
            string referTo, bool toSelf, Dictionary<string, string>? variables = null)
        {
            var pathWithQuery = new QueryBuilder($"endpoints/{tech}/{resource}/refer")
                .Add("from", from)
                .Add("refer_to", referTo)
                .Add("to_self", toSelf);
            var data = new
            {
                variables
            };
            var response = await httpClient.PostAsJsonAsync(pathWithQuery.ToString(), data);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Invalid parameters for referring.", (int)response.StatusCode),
                    404 => new AsterAriException("Endpoint not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }
    }
}