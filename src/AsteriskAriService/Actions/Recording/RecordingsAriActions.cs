using System.Net.Http.Json;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Actions.Recording
{
    public class RecordingsAriActions(
        HttpClient httpClient
        ) : IRecordingsAriActions
    {
        public async Task<List<StoredRecordingModel>> ListStoredAsync()
        {
            var response = await httpClient.GetAsync("recordings/stored");
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<List<StoredRecordingModel>>())!;
        }

        public async Task<StoredRecordingModel> GetStoredAsync(string recordingName)
        {
            var response = await httpClient.GetAsync($"recordings/stored/{recordingName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<StoredRecordingModel>())!;
        }

        public async Task<StoredRecordingModel?> TryGetStoredAsync(string recordingName)
        {
            try
            {
                return await GetStoredAsync(recordingName);
            }
            catch (Exception)
            {
                // ignored
                return null;
            }
        }

        public async Task DeleteStoredAsync(string recordingName)
        {
            var response = await httpClient.DeleteAsync($"recordings/stored/{recordingName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task<byte[]> GetStoredFileAsync(string recordingName)
        {
            var response = await httpClient.GetAsync($"recordings/stored/{recordingName}/file");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    403 => new AsterAriException("The recording file could not be opened", (int)response.StatusCode),
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<StoredRecordingModel> CopyStoredAsync(string recordingName, string destinationRecordingName)
        {
            var response = await httpClient.PostAsync($"recordings/stored/{recordingName}/copy", null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    409 => new AsterAriException("A recording with the same name already exists on the system", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<StoredRecordingModel>())!;
        }

        public async Task<LiveRecordingModel> GetLiveAsync(string recordingName)
        {
            var response = await httpClient.GetAsync($"recordings/live/{recordingName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<LiveRecordingModel>())!;
        }

        public async Task CancelAsync(string recordingName)
        {
            var response = await httpClient.DeleteAsync($"recordings/live/{recordingName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task StopAsync(string recordingName)
        {
            var response = await httpClient.PostAsync($"recordings/live/{recordingName}/stop", null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task PauseAsync(string recordingName)
        {
            var response = await httpClient.PostAsync($"recordings/live/{recordingName}/pause", null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    409 => new AsterAriException("Recording not in session", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task UnpauseAsync(string recordingName)
        {
            var response = await httpClient.DeleteAsync($"recordings/live/{recordingName}/pause");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    409 => new AsterAriException("Recording not in session", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task MuteAsync(string recordingName)
        {
            var response = await httpClient.PostAsync($"recordings/live/{recordingName}/mute", null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    409 => new AsterAriException("Recording not in session", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task UnmuteAsync(string recordingName)
        {
            var response = await httpClient.DeleteAsync($"recordings/live/{recordingName}/mute");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Recording not found", (int)response.StatusCode),
                    409 => new AsterAriException("Recording not in session", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }
    }
}