using System.Net.Http.Json;
using System.Text.Json;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Asterisk.AsteriskInfo;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Tools;

namespace AsteriskAriService.Actions.Asterisk
{
    public class AsteriskAriActions(HttpClient httpClient): IAsteriskAriActions
    {
        public async Task<List<ConfigTupleModel>> GetObjectAsync(string configClass, string objectType, string id)
        {
            var response = await httpClient.GetAsync($"asterisk/config/dynamic/{configClass}/{objectType}/{id}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("{configClass|objectType|id} not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<List<ConfigTupleModel>>())!;
        }

        public async Task<List<ConfigTupleModel>> UpdateObjectAsync(string configClass, string objectType, string id, Dictionary<string, string>? fields = null)
        {
            var response = await httpClient.PutAsJsonAsync($"asterisk/config/dynamic/{configClass}/{objectType}/{id}", fields);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Bad request body", (int)response.StatusCode),
                    403 => new AsterAriException("Could not create or update object", (int)response.StatusCode),
                    404 => new AsterAriException("{configClass|objectType} not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<List<ConfigTupleModel>>())!;
        }

        public async Task DeleteObjectAsync(string configClass, string objectType, string id)
        {
            var response = await httpClient.DeleteAsync($"asterisk/config/dynamic/{configClass}/{objectType}/{id}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    403 => new AsterAriException("Could not delete object", (int)response.StatusCode),
                    404 => new AsterAriException("{configClass|objectType|id} not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task<AsteriskInfoModel> GetInfoAsync(string? only = null)
        {
            var pathWithQuery = new QueryBuilder("asterisk/info")
                .Add("only", only);
            var response = await httpClient.GetAsync(pathWithQuery.ToString());
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<AsteriskInfoModel>(GetJsonOptions()))!;
        }

        public async Task<AsteriskPingModel> PingAsync()
        {
            var response = await httpClient.GetAsync("asterisk/ping");
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<AsteriskPingModel>(GetJsonOptions()))!;
        }

        public async Task<List<ModuleModel>> ListModulesAsync()
        {
            var response = await httpClient.GetAsync("asterisk/modules");
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<List<ModuleModel>>(GetJsonOptions()))!;
        }

        public async Task<ModuleModel> GetModuleAsync(string moduleName)
        {
            var response = await httpClient.GetAsync($"asterisk/modules/{moduleName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Module could not be found in running modules.", (int)response.StatusCode),
                    409 => new AsterAriException("Module information could not be retrieved.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<ModuleModel>(GetJsonOptions()))!;
        }

        public async Task LoadModuleAsync(string moduleName)
        {
            var response = await httpClient.PostAsync($"asterisk/modules/{moduleName}", null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    409 => new AsterAriException("Module could not be loaded.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task UnloadModuleAsync(string moduleName)
        {
            var response = await httpClient.DeleteAsync($"asterisk/modules/{moduleName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Module not found in running modules.", (int)response.StatusCode),
                    409 => new AsterAriException("Module could not be unloaded.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task ReloadModuleAsync(string moduleName)
        {
            var response = await httpClient.PutAsync($"asterisk/modules/{moduleName}", null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Module not found in running modules.", (int)response.StatusCode),
                    409 => new AsterAriException("Module could not be reloaded.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task<List<LogChannelModel>> ListLogChannelsAsync()
        {
            var response = await httpClient.GetAsync("asterisk/logging");
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<List<LogChannelModel>>(GetJsonOptions()))!;
        }

        public async Task AddLogAsync(string logChannelName, string configuration)
        {
            var pathWithQuery = new QueryBuilder($"asterisk/logging/{logChannelName}")
                .Add("configuration", configuration);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Bad request body", (int)response.StatusCode),
                    409 => new AsterAriException("Log channel could not be created.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task DeleteLogAsync(string logChannelName)
        {
            var response = await httpClient.DeleteAsync($"asterisk/logging/{logChannelName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Log channel does not exist.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task RotateLogAsync(string logChannelName)
        {
            var response = await httpClient.PutAsync($"asterisk/logging/{logChannelName}/rotate", null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Log channel does not exist.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task<VariableModel> GetGlobalVarAsync(string variable)
        {
            var pathWithQuery = new QueryBuilder("asterisk/variable")
                .Add("variable", variable);
            var response = await httpClient.GetAsync(pathWithQuery.ToString());
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Missing variable parameter.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<VariableModel>(GetJsonOptions()))!;
        }

        public async Task SetGlobalVarAsync(string variable, string? value = null)
        {
            var pathWithQuery = new QueryBuilder("asterisk/variable")
                .Add("variable", variable)
                .Add("value", value);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Missing variable parameter.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
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