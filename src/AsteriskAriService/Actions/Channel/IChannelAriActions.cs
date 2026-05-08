using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Models.Number;

namespace AsteriskAriService.Actions.Channel
{
    public interface IChannelAriActions
    {
	    /// <summary>
	    /// List all active channels in Asterisk.
	    /// </summary>
	    /// <returns></returns>
	    Task<List<ChannelModel>> ListAsync();
	    
	    /// <summary>
	    /// Create a new channel (originate).
	    /// The new channel is created immediately and a snapshot of it returned.
	    /// If a Stasis application is provided it will be automatically subscribed
	    /// to the originated channel for further events and updates.
	    /// </summary>
	    /// <param name="endpoint">Endpoint to call</param>
	    /// <param name="extension"> The extension to dial after the endpoint answers. Mutually exclusive with 'app'.</param>
	    /// <param name="context"> The context to dial after the endpoint answers. If omitted, uses 'default'. Mutually exclusive with 'app'.</param>
	    /// <param name="priority">The priority to dial after the endpoint answers. If omitted, uses 1. Mutually exclusive with 'app'.</param>
	    /// <param name="label"> The label to dial after the endpoint answers. Will supersede 'priority' if provided. Mutually exclusive with 'app'.</param>
	    /// <param name="app">- The application that is subscribed to the originated channel. When the channel is answered, it will be passed to this Stasis application. Mutually exclusive with 'context', 'extension', 'priority', and 'label'.</param>
	    /// <param name="appArgs"> The application arguments to pass to the Stasis application provided by 'app'. Mutually exclusive with 'context', 'extension', 'priority', and 'label'.</param>
	    /// <param name="callerId"> CallerID to use when dialing the endpoint or extension.</param>
	    /// <param name="timeout">Timeout (in seconds) before giving up dialing, or -1 for no timeout. Default: 30</param>
	    /// <param name="variables">The "variables" key in the body object holds variable key/value pairs to set on the channel on creation. "CALLERID(name)": "Alice"</param>
	    /// <param name="channelId">The unique id to assign the channel on creation.</param>
	    /// <param name="otherChannelId">The unique id to assign the second channel when using local channels.</param>
	    /// <param name="originator"> The unique id of the channel which is originating this one.</param>
	    /// <param name="formats">The format name capability list to use if originator is not specified. Ex. "ulaw,slin16". Format names can be found with "core show codecs"</param>
	    /// <exception cref="AsterAriException">400 - Invalid parameters for originating a channel.</exception>
	    /// <exception cref="AsterAriException">409 - Channel with given unique ID already exists.</exception>
	    /// <returns></returns>
	    Task<ChannelModel> OriginateAsync(
		    AriNumber endpoint,
		    string? extension = null,
		    string? context = null,
		    long? priority = null,
		    string? label = null,
		    string? app = null,
		    string? appArgs = null,
		    string? callerId = null,
		    int? timeout = null,
		    Dictionary<string, string>? variables = null,
		    string? channelId = null,
		    string? otherChannelId = null,
		    string? originator = null,
		    string? formats = null);
	    
	    /// <summary>
	    /// Create a new channel (originate). The new channel is created immediately and a snapshot of it returned.
	    /// If a Stasis application is provided it will be automatically subscribed to the originated channel for further events and updates.
	    /// </summary>
	    /// <param name="endpoint">Endpoint to call.</param>
	    /// <param name="app">The application that is subscribed to the originated channel.
	    /// When the channel is answered, it will be passed to this Stasis application.
	    /// Mutually exclusive with 'context', 'extension', 'priority', and 'label'.
	    /// If you do not specify it, the data from the connection will be used.
	    /// </param>
	    /// <param name="appArgs">The application arguments to pass to the Stasis application provided by 'app'.
	    /// Mutually exclusive with 'context', 'extension', 'priority', and 'label'.</param>
	    /// <param name="channelId">The unique id to assign the channel on creation.</param>
	    /// <param name="otherChannelId">The unique id to assign the second channel when using local channels.</param>
	    /// <param name="originator">The unique id of the channel which is originating this one.</param>
	    /// <param name="formats">The format name capability list to use if originator is not specified.
	    /// Ex. "ulaw,slin16".  Format names can be found with "core show codecs".</param>
	    /// <param name="variables">The "variables" key in the body object holds variable key/value pairs to set on the channel on creation.
	    /// Other keys in the body object are interpreted as query parameters.
	    /// Ex. { "endpoint": "SIP/Alice", "variables": { "CALLERID(name)": "Alice" } }</param>
	    /// <exception cref="AsterAriException">409 - Channel with given unique ID already exists.</exception>
	    public Task<ChannelModel> CreateAsync(AriNumber endpoint, string? app = null, 
		    string? appArgs = null, string? channelId = null,
		    string? otherChannelId = null, string? originator = null, 
		    string? formats = null,Dictionary<string, string>? variables = null);
	    
	    /// <summary>
	    /// Channel details.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <returns></returns>
	    Task<ChannelModel> GetAsync(string channelId);
	    
	    /// <summary>
	    /// Delete (i.e. hangup) a channel.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <param name="reason">
	    /// Reason for hanging up the channel for simple use. Mutually exclusive with 'reason_code'.
	    /// Allowed values: normal, busy, congestion, no_answer, timeout, rejected, unallocated, normal_unspecified, number_incomplete, codec_mismatch, interworking, failure, answered_elsewhere
	    /// </param>
	    /// <exception cref="AsterAriException">400 - Invalid reason for hangup provided</exception>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <returns></returns>
	    Task HangupAsync(string channelId, string? reason = null);
        
	    /// <inheritdoc cref="HangupAsync"/>>
	    Task TryHangupAsync(string channelId, string? reason = null);
	    
	    /// <summary>
	    /// Exit application; continue execution in the dialplan.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <param name="context">The context to continue to.</param>
	    /// <param name="extension">The extension to continue to.</param>
	    /// <param name="priority">The priority to continue to.</param>
	    /// <param name="label"> The label to continue to - will supersede 'priority' if both are provided.</param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task ContinueInDialplanAsync(
		    string channelId,
		    string? context = null,
		    string? extension = null,
		    int? priority = null,
		    string? label = null);
	    
	    /// <summary>
	    /// Move the channel from one Stasis application to another.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <param name="app">The channel will be passed to this Stasis application.</param>
	    /// <param name="appArgs"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <returns></returns>
	    Task MoveAsync(string channelId,string app, string appArgs);
	    
	    /// <summary>
	    /// Redirect the channel to a different location.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <param name="endpoint"></param>
	    /// <exception cref="AsterAriException">400 - Endpoint parameter not provided</exception>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <exception cref="AsterAriException">422 - Endpoint is not the same type as the channel</exception>
	    /// <returns></returns>
	    Task RedirectAsync(string channelId, AriNumber endpoint);
	    
	    /// <summary>
	    /// Answer a channel. 
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
        Task AnswerAsync(string channelId);
        
	    /// <summary>
	    /// Indicate ringing to a channel.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
        Task RingAsync(string channelId);

	    /// <summary>
	    /// Stop ringing indication on a channel if locally generated.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
        Task RingStopAsync(string channelId);
        
	    /// <summary>
	    /// Indicate progress on a channel.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task ProgressAsync(string channelId);
	    
	    /// <summary>
	    /// Send provided DTMF to a given channel.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <param name="dtmf">DTMF To send.</param>
	    /// <param name="before">Amount of time to wait before DTMF digits (specified in milliseconds) start.</param>
	    /// <param name="between">Amount of time in between DTMF digits (specified in milliseconds).Default: 100</param>
	    /// <param name="duration">Length of each DTMF digit (specified in milliseconds).Default: 100</param>
	    /// <param name="after"> Amount of time to wait after DTMF digits (specified in milliseconds) end.</param>
	    /// <exception cref="AsterAriException">400 - DTMF is required</exception>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task SendDtmfAsync(
		    string channelId,
		    string dtmf,
		    int? before = null,
		    int? between = null,
		    int? duration = null,
		    int? after = null);
	    
	    /// <summary>
	    /// Mute a channel
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <param name="direction">Direction in which to mute audio Default: both. Allowed values: both, in, out</param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task MuteAsync(string channelId, string direction);

	    /// <summary>
	    /// Unmute a channel.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <param name="direction">Direction in which to unmute audio. Default: both. Allowed values: both, in, out</param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task UnmuteAsync(string channelId, string direction);

	    /// <summary>
	    /// Hold a channel.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task HoldAsync(string channelId);

	    /// <summary>
	    /// Remove a channel from hold.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task UnholdAsync(string channelId);

	    /// <summary>
	    /// Play music on hold to a channel.
	    /// Using media operations such as /play on a channel playing MOH in this manner will suspend MOH
	    /// without resuming automatically. If continuing music on hold is desired,
	    /// the stasis application must reinitiate music on hold.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <param name="mohClass"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task StartMohAsync(string channelId, string mohClass);

	    /// <summary>
	    /// Stop playing music on hold to a channel.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task StopMohAsync(string channelId);

	    /// <summary>
	    /// Play silence to a channel.
	    /// Using media operations such as /play on a channel playing silence in this manner will
	    /// suspend silence without resuming automatically.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task StartSilenceAsync(string channelId);

	    /// <summary>
	    /// Stop playing silence to a channel.
	    /// </summary>
	    /// <param name="channelId"></param>
	    /// <exception cref="AsterAriException">404 - Channel not found</exception>
	    /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
	    /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
	    /// <returns></returns>
	    Task StopSilenceAsync(string channelId);
        
        /// <summary>
        /// Start playback of media. The media URI may be any of a number of URI's.
        /// Currently sound:, recording:, number:, digits:, characters:, and tone: URI's are supported.
        /// This operation creates a playback resource that can be used to control the playback of media (pause, rewind, fast-forward, etc.)
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="media">Media URIs to play. Allows comma separated values.</param>
        /// <param name="lang">For sounds, selects language for sound.</param>
        /// <param name="offsetMs">Number of milliseconds to skip before playing. Only applies to the first URI if multiple media URIs are specified.</param>
        /// <param name="skipMs">Number of milliseconds to skip for forward/reverse operations. Default: 3000</param>
        /// <param name="playbackId"></param>
        /// <exception cref="AsterAriException">404 - Channel not found</exception>
        /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
        /// <exception cref="AsterAriException">412 - Channel in invalid state</exception>
        /// <returns></returns>
        Task<PlaybackModel> PlayAsync(
            string channelId,
            string media,
            string? lang = null,
            int? offsetMs = null,
            int? skipMs = null,
            string? playbackId = null);
        
        /// <summary>
        /// Start a recording. Record audio from a channel.
        /// Note that this will not capture audio sent to the channel.
        /// The bridge itself has a record feature if that's what you want.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="name">Recording's filename</param>
        /// <param name="format">Format to encode audio in (wav, gsm, etc.)</param>
        /// <param name="maxDurationSeconds">Maximum duration of the recording, in seconds. 0 for no limit</param>
        /// <param name="maxSilenceSeconds">Maximum duration of silence, in seconds. 0 for no limit</param>
        /// <param name="ifExists">Action to take if a recording with the same name already exists. Default: fail.
        /// Allowed values: fail, overwrite, append</param>
        /// <param name="beep">Play beep when recording begins</param>
        /// <param name="terminateOn">DTMF input to terminate recording. Default: none.
        /// Allowed values: none, any, *, #</param>
        /// <exception cref="AsterAriException">400 - Invalid parameters</exception>
        /// <exception cref="AsterAriException">404 - Channel not found</exception>
        /// <exception cref="AsterAriException">409 - Channel not in a Stasis application;
        /// the channel is currently bridged with other channels;
        /// A recording with the same name already exists on the system and can not be overwritten
        /// because it is in progress or ifExists=fail</exception>
        /// <exception cref="AsterAriException">422 - The format specified is unknown on this system</exception>
        /// <returns></returns>
        Task<LiveRecordingModel> RecordAsync(
	        string channelId,
	        string name,
	        string format,
	        int? maxDurationSeconds = null,
	        int? maxSilenceSeconds = null,
	        string ifExists = "fail",
	        bool? beep = null,
	        string? terminateOn = null);
        
        /// <summary>
        /// Get the value of a channel variable or function.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="variable"> The channel variable or function to get</param>
        /// <exception cref="AsterAriException">400 - Missing variable parameter</exception>
        /// <exception cref="AsterAriException">404 - Channel or variable not found</exception>
        /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
        /// <returns></returns>
        Task<VariableModel> GetChannelVarAsync(string channelId, string variable);
        
        /// <summary>
        /// Set the value of a channel variable or function.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="variable">The channel variable or function to set</param>
        /// <param name="value">The value to set the variable to</param>
        /// <exception cref="AsterAriException">400 - Missing variable parameter</exception>
        /// <exception cref="AsterAriException">404 - Channel not found</exception>
        /// <exception cref="AsterAriException">409 - Channel not in a Stasis application</exception>
        /// <returns></returns>
        Task SetChannelVarAsync(string channelId, string variable, string? value = null);
        
        /// <summary>
        /// Start snooping. Snoop (spy/whisper) on a specific channel.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="app">Application the snooping channel is placed into</param>
        /// <param name="spy">Direction of audio to spy on Default: none.
        /// Allowed values: none, both, out, in</param>
        /// <param name="whisper">Direction of audio to whisper into.Default: none.
        /// Allowed values: none, both, out, in</param>
        /// <param name="appArgs"> The application arguments to pass to the Stasis application</param>
        /// <param name="snoopId">Unique ID to assign to snooping channel</param>
        /// <exception cref="AsterAriException">400 - Invalid parameters</exception>
        /// <exception cref="AsterAriException">404 - Channel not found</exception>
        /// <returns></returns>
        Task<ChannelModel> SnoopChannelAsync(
	        string channelId,
	        string app,
	        string? spy = null,
	        string? whisper = null,
	        string? appArgs = null,
	        string? snoopId = null);
        
        /// <summary>
        /// Dial a created channel.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="caller">Channel ID of caller</param>
        /// <param name="timeout">Allowed range: Min: 0; Max: None</param>
        /// <exception cref="AsterAriException">404 - Channel cannot be found.</exception>
        /// <exception cref="AsterAriException">409 - Channel cannot be dialed.</exception>
        /// <returns></returns>
        Task DialAsync(string channelId, string? caller = null, int? timeout = null);
        
        /// <summary>
        /// RTP stats on a channel.
        /// </summary>
        /// <param name="channelId">Channel's id</param>
        /// <exception cref="AsterAriException">404 - Channel cannot be found.</exception>
        Task<RtpStatModel> RtpStatisticsAsync(string channelId);
        
        /// <summary>
        /// Start an External Media session. Create a channel to an External Media source/sink.
        /// </summary>
        /// <param name="channelId">The unique id to assign the channel on creation.</param>
        /// <param name="app">Stasis Application to place channel into</param>
        /// <param name="variables">The "variables" key in the body object holds variable key/value pairs to set on the channel on creation. Other keys in the body object are interpreted as query parameters. Ex. { "endpoint": "SIP/Alice", "variables": { "CALLERID(name)": "Alice" } }</param>
        /// <param name="externalHost">Hostname/ip:port of external host</param>
        /// <param name="encapsulation">Payload encapsulation protocol</param>
        /// <param name="transport">Transport protocol</param>
        /// <param name="connectionType">Connection type (client/server)</param>
        /// <param name="format">Format to encode audio in</param>
        /// <param name="direction">External media direction</param>
        /// <param name="data">An arbitrary data field</param>
        /// <param name="transportData">Transport-specific data. For websocket this is appended to the dialstring.</param>
        /// <exception cref="AsterAriException">400 - Invalid parameters</exception>
        /// <exception cref="AsterAriException">409 - Channel is not in a Stasis application; Channel is already bridged</exception>
        Task<ChannelModel> ExternalMediaAsync(string? app, string externalHost, string format,
	        string? channelId = null, Dictionary<string, string>? variables = null,
	        string? encapsulation = null, string? transport = null, string? connectionType = null,
	        string? direction = null, string? data = null, string? transportData = null);
    }
}