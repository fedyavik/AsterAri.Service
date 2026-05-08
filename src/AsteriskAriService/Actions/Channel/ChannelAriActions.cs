using System.Net.Http.Json;
using System.Text.Json;
using AsteriskAriService.Models;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Models.Number;
using AsteriskAriService.Tools;
using Microsoft.Extensions.Options;

namespace AsteriskAriService.Actions.Channel
{
    public class ChannelAriActions(
        HttpClient httpClient,
        IOptions<ServerAriOptions> serverOptions): IChannelAriActions
    {
        public async Task<List<ChannelModel>> ListAsync()
        {
            var response = await httpClient.GetAsync("channels");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<List<ChannelModel>>(GetJsonOptions()))!;
        }
        
        public async Task<ChannelModel> OriginateAsync(AriNumber endpoint, string? extension = null, string? context = null, long? priority = null,
            string? label = null, string? app = null, string? appArgs = null, string? callerId = null, int? timeout = null,
            Dictionary<string, string>? variables = null, string? channelId = null, string? otherChannelId = null, string? originator = null,
            string? formats = null)
        {
            variables ??= new Dictionary<string, string>();
            if (!variables.TryGetValue("CALLERID(num)", out var _))
                variables["CALLERID(num)"] = endpoint.Number;
            var data = new
            {
                endpoint = endpoint.Endpoint,
                extension,
                context,
                priority,
                label,
                app,
                appArgs,
                callerId,
                timeout,
                variables,
                channelId,
                otherChannelId,
                originator,
                formats,
            };
            var response = await httpClient.PostAsJsonAsync("channels", data);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException(" Invalid parameters for originating a channel.", (int)response.StatusCode),
                    409 => new AsterAriException("Channel with given unique ID already exists.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<ChannelModel>(GetJsonOptions()))!;
        }
        
        public async Task<ChannelModel> CreateAsync(AriNumber endpoint, string? app, string? appArgs = null, string? channelId = null,
            string? otherChannelId = null, string? originator = null, string? formats = null, Dictionary<string, string>? variables = null)
        {
            variables ??= new Dictionary<string, string>();
            if (!variables.TryGetValue("CALLERID(num)", out var _))
                variables["CALLERID(num)"] = endpoint.Number;
            var data = new
            {
                endpoint = endpoint.Endpoint,
                app = app ?? serverOptions.Value.ConnectOptions.AppName,
                appArgs,
                channelId,
                otherChannelId,
                originator,
                formats,
                variables,
            };
            var response = await httpClient.PostAsJsonAsync("channels/create", data);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    409 => new AsterAriException("Channel with given unique ID already exists.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<ChannelModel>(GetJsonOptions()))!;
        }

        public async Task<ChannelModel> GetAsync(string channelId)
        {
            var response = await httpClient.GetAsync($"channels/{channelId}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<ChannelModel>(GetJsonOptions()))!;
        }
        
        public async Task HangupAsync(string channelId, string? reason = null)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}")
                .Add("reason", reason);
            var response = await httpClient.DeleteAsync(pathWithQuery.ToString());
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400  => new AsterAriException("Invalid reason for hangup provided", (int)response.StatusCode),
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task TryHangupAsync(string channelId, string? reason = null)
        {
            try
            {
                await HangupAsync(channelId, reason);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public async Task ContinueInDialplanAsync(string channelId, string? context = null, string? extension = null, int? priority = null,
            string? label = null)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/continue")
                .Add("context", context)
                .Add("extension", extension)
                .Add("priority", priority)
                .Add("label", label);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task MoveAsync(string channelId,string app, string appArgs)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/move")
                .Add("app", app)
                .Add("appArgs", appArgs);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task RedirectAsync(string channelId, AriNumber endpoint)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/redirect")
                .Add("endpoint", endpoint.Endpoint);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400  => new AsterAriException("Endpoint parameter not provided", (int)response.StatusCode),
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    422 => new AsterAriException("Endpoint is not the same type as the channel", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task AnswerAsync(string channelId)
        {
            var path = $"channels/{channelId}/answer";
            var response = await httpClient.PostAsync(path, null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task RingAsync(string channelId)
        {
            var path = $"channels/{channelId}/ring";
            var response = await httpClient.PostAsync(path, null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task RingStopAsync(string channelId)
        {
            var path = $"channels/{channelId}/ring";
            var response = await httpClient.DeleteAsync(path);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task ProgressAsync(string channelId)
        {
            var path = $"channels/{channelId}/progress";
            var response = await httpClient.PostAsync(path, null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task SendDtmfAsync(string channelId, string dtmf, int? before = null, int? between = null, int? duration = null,
            int? after = null)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/dtmf")
                .Add("dtmf", dtmf)
                .Add("before", before)
                .Add("between", between)
                .Add("duration", duration)
                .Add("after", after) ;
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400  => new AsterAriException("DTMF is required", (int)response.StatusCode),
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task MuteAsync(string channelId, string direction)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/mute")
                .Add("direction", direction);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task UnmuteAsync(string channelId, string direction)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/mute")
                .Add("direction", direction);
            var response = await httpClient.DeleteAsync(pathWithQuery.ToString());
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task HoldAsync(string channelId)
        {
            var response = await httpClient.PostAsync($"channels/{channelId}/hold", null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task UnholdAsync(string channelId)
        {
            var response = await httpClient.DeleteAsync($"channels/{channelId}/hold");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task StartMohAsync(string channelId, string mohClass)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/moh")
                .Add("mohClass", mohClass);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task StopMohAsync(string channelId)
        {
            var response = await httpClient.DeleteAsync($"channels/{channelId}/moh");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task StartSilenceAsync(string channelId)
        {
            var response = await httpClient.PostAsync($"channels/{channelId}/silence", null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task StopSilenceAsync(string channelId)
        {
            var response = await httpClient.DeleteAsync($"channels/{channelId}/silence");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    412 => new AsterAriException("Channel in invalid state", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
        }

        public async Task<PlaybackModel> PlayAsync(string channelId, string media, string? lang = null, 
            int? offsetMs = null, int? skipMs = null, string? playbackId = null)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/play")
                .Add("media", media)
                .Add("lang", lang)
                .Add("offsetms", offsetMs)
                .Add("skipms", skipMs)
                .Add("playbackId", playbackId);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<PlaybackModel>(GetJsonOptions()))!;
            throw (int)response.StatusCode switch
            {
                404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                412  => new AsterAriException(" Channel in invalid state", (int)response.StatusCode),
                _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
            };
        }
        
        public async Task<LiveRecordingModel> RecordAsync(string channelId, string name, string format, int? maxDurationSeconds = null,
            int? maxSilenceSeconds = null, string ifExists = "fail", bool? beep = null, string? terminateOn = null)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/record")
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
                    400  => new AsterAriException("Invalid parameters", (int)response.StatusCode),
                    404  => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    422 => new AsterAriException("The format specified is unknown on this system or ifExists=fail", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<LiveRecordingModel>(GetJsonOptions()))!;
        }

        public async Task<VariableModel> GetChannelVarAsync(string channelId, string variable)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/variable")
                .Add("variable", variable);
            var response = await httpClient.GetAsync(pathWithQuery.ToString());
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400  => new AsterAriException("Missing variable parameter", (int)response.StatusCode),
                    404  => new AsterAriException("Channel or variable not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.",
                        (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<VariableModel>())!;
        }

        public async Task SetChannelVarAsync(string channelId, string variable, string? value = null)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/variable")
                .Add("variable", variable)
                .Add("value", value);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400  => new AsterAriException("Missing variable parameter.", (int)response.StatusCode),
                    404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel not in a Stasis application", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task<ChannelModel> SnoopChannelAsync(string channelId, string app, string? spy = null, string? whisper = null, string? appArgs = null,
            string? snoopId = null)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/snoop")
                .Add("app", app)
                .Add("spy", spy)
                .Add("whisper", whisper)
                .Add("appArgs", appArgs)
                .Add("snoopId", snoopId);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Invalid parameters", (int)response.StatusCode),
                    404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<ChannelModel>(GetJsonOptions()))!;
        }

        public async Task DialAsync(string channelId, string? caller = null, int? timeout = null)
        {
            var pathWithQuery = new QueryBuilder($"channels/{channelId}/dial")
                .Add("caller", caller)
                .Add("timeout", timeout);
            var response = await httpClient.PostAsync(pathWithQuery.ToString(), null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    409 => new AsterAriException("Channel cannot be dialed", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task<RtpStatModel> RtpStatisticsAsync(string channelId)
        {
            var response = await httpClient.GetAsync($"channels/{channelId}/rtp_statistics");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Channel not found", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<RtpStatModel>())!;
        }

        public async Task<ChannelModel> ExternalMediaAsync(string? app, string externalHost, string format, string? channelId = null,
            Dictionary<string, string>? variables = null, string? encapsulation = null, string? transport = null, string? connectionType = null,
            string? direction = null, string? data = null, string? transportData = null)
        {
            var dataSend = new
            {
                channelId,
                app = app ?? serverOptions.Value.ConnectOptions.AppName,
                external_host = externalHost,
                encapsulation,
                transport,
                connection_type = connectionType,
                format,
                direction,
                data,
                transport_data = transportData,
                variables,
            };
            var response = await httpClient.PostAsJsonAsync("channels/externalMedia", dataSend);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    400 => new AsterAriException("Invalid parameters", (int)response.StatusCode),
                    409 => new AsterAriException("Channel is not in a Stasis application; Channel is already bridged", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<ChannelModel>())!;
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