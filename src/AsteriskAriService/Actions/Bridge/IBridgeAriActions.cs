using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Actions.Bridge
{
    public interface IBridgeAriActions
    {
        /// <summary>
        /// List all active bridges in Asterisk.
        /// </summary>
        Task<List<BridgeModel>> ListAsync();
        /// <summary>
        /// Create a new bridge. This bridge persists until it has been shut down, or Asterisk has been shut down.
        /// </summary>
        /// <param name="type">Comma separated list of bridge type attributes
        /// (mixing, holding, dtmf_events, proxy_media, video_sfu).</param>
        /// <param name="bridgeId">Unique ID to give to the bridge being created.</param>
        /// <param name="name">Name to give to the bridge being created.</param>
        Task<BridgeModel> CreateAsync(string? type = null, string? bridgeId = null, string? name = null);
        
        /// <summary>
        /// Get bridge details.
        /// </summary>
        /// <param name="bridgeId"></param>
        /// <exception cref="AsterAriException">404 - Bridge not found</exception>
        /// <returns></returns>
        Task<BridgeModel> GetAsync(string bridgeId);
        /// <summary>
        /// disable all attached channels
        /// </summary>
        /// <param name="bridgeId"></param>
        /// <returns></returns>
        Task HangupConnectedChannelsAsync(string bridgeId);
        /// <summary>
        /// Play music on hold to a bridge or change the MOH class that is playing.
        /// <exception cref="AsterAriException">404 - Bridge  not found</exception>
        /// <exception cref="AsterAriException">409 - Bridge not in Stasis application</exception>
        /// </summary>
        Task StartMohAsync(string bridgeId, string? mohClass = null);
        
        /// <summary>
        /// Stop playing music on hold to a bridge. This will only stop music on hold being played via POST bridges/{bridgeId}/moh.
        /// </summary>
        /// <param name="bridgeId"></param>
        /// <exception cref="AsterAriException">404 - Bridge  not found</exception>
        /// <exception cref="AsterAriException">409 - Bridge not in Stasis application</exception>
        /// <returns></returns>
        Task StopMohAsync(string bridgeId);
        
        /// <summary>
        /// Add a channels to a bridge.
        /// </summary>
        /// <param name="bridgeId"></param>
        /// <param name="channelsIds">Ids of channels to add to bridge</param>
        /// <param name="role">Channel's role in the bridge</param>
        /// <param name="absorbDtmf"> Absorb DTMF coming from this channel, preventing it to pass through to the bridge</param>
        /// <param name="mute"> Mute audio from this channel, preventing it to pass through to the bridge</param>
        /// <param name="inhibitConnectedLineUpdates"> Mute audio from this channel, preventing it to pass through to the bridge</param>
        /// <exception cref="AsterAriException">400 - Channel not found</exception>
        /// <exception cref="AsterAriException">404 - Bridge  not found</exception>
        /// <exception cref="AsterAriException">409 - Bridge not in Stasis application; Channel currently recording</exception>
        /// <exception cref="AsterAriException">422 - Channel not in Stasis application</exception>
        /// <returns></returns>
        Task AddChannelAsync(string bridgeId, string? role = null,
            bool absorbDtmf = false, bool mute = false,
            bool inhibitConnectedLineUpdates = true,
            params string[] channelsIds);
        
        /// <inheritdoc cref="AddChannelAsync"/>>
        Task TryAddChannelAsync(string bridgeId, string? role = null,
            bool absorbDtmf = false, bool mute = false,
            bool inhibitConnectedLineUpdates = true,
            params string[] channelsIds);
        
        /// <summary>
        /// Remove a channel from a bridge.
        /// </summary>
        /// <param name="bridgeId"></param>
        /// <param name="channelsIds"> Ids of channels to remove from bridge</param>
        /// <exception cref="AsterAriException">400 - Channel not found</exception>
        /// <exception cref="AsterAriException">404 - Bridge  not found</exception>
        /// <exception cref="AsterAriException">409 - Bridge not in Stasis application; Channel currently recording</exception>
        /// <exception cref="AsterAriException">422 - Channel not in Stasis application</exception>
        /// <returns></returns>
        Task RemoveChannelAsync(string bridgeId, params string[] channelsIds);
        
        /// <inheritdoc cref="RemoveChannelAsync"/>>
        Task TryRemoveChannelAsync(string bridgeId, params string[] channelsIds);
        
        /// <summary>
        /// Shut down a bridge. If any channels are in this bridge, they will be removed and resume whatever they were doing beforehand.
        /// </summary>
        /// <param name="bridgeId"></param>
        /// <exception cref="AsterAriException">404 - Bridge not found</exception>
        /// <returns></returns>
        Task DestroyAsync(string bridgeId);
                
        /// <inheritdoc cref="DestroyAsync"/>>
        Task TryDestroyAsync(string bridgeId);
        
        /// <summary>
        /// Start a recording. This records the mixed audio from all channels participating in this bridge.
        /// </summary>
        /// <param name="bridgeId"></param>
        /// <param name="name">Recording's filename</param>
        /// <param name="format">Format to encode audio in (wav, gsm, etc.)</param>
        /// <param name="maxDurationSeconds">Maximum duration of the recording, in seconds. 0 for no limit. Allowed range: Min: 0; Max: None</param>
        /// <param name="maxSilenceSeconds"> Maximum duration of silence, in seconds. 0 for no limit. Allowed range: Min: 0; Max: None</param>
        /// <param name="ifExists">Action to take if a recording with the same name already exists.
        /// Default: fail.
        /// Allowed values: fail, overwrite, append</param>
        /// <param name="beep">Play beep when recording begins</param>
        /// <param name="terminateOn">DTMF input to terminate recording.
        /// Default: none.
        /// Allowed values: none, any, *, #</param>
        /// <exception cref="AsterAriException">400 - Invalid parameters</exception>
        /// <exception cref="AsterAriException">404 - Bridge not found</exception>
        /// <exception cref="AsterAriException">409 - Bridge is not in a Stasis application;
        /// A recording with the same name already exists on the system
        /// and can not be overwritten because it is in progress or ifExists=fail</exception>
        /// <exception cref="AsterAriException">422 -  The format specified is unknown on this system</exception>
        /// <returns></returns>
        Task<LiveRecordingModel> RecordAsync(
            string bridgeId, string name, string format, int? maxDurationSeconds = null,
            int? maxSilenceSeconds = null, string? ifExists = null, bool? beep = null, string? terminateOn = null);
    }
}