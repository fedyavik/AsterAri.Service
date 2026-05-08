using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Actions.Playback
{
    public interface IPlaybackAriActions
    {
        /// <summary>
        /// Get a playback's details.
        /// </summary>
        /// <param name="playbackId">Playback's id</param>
        /// <exception cref="AsterAriException">404 - The playback cannot be found</exception>
        Task<PlaybackModel> GetAsync(string playbackId);
        
        /// <summary>
        /// Stop a playback.
        /// </summary>
        /// <param name="playbackId">Playback's id</param>
        /// <exception cref="AsterAriException">404 - The playback cannot be found</exception>
        Task StopAsync(string playbackId);
        
        /// <summary>
        /// Control a playback.
        /// </summary>
        /// <param name="playbackId">Playback's id</param>
        /// <param name="operation">Operation to perform on the playback.
        /// Allowed values: restart, pause, unpause, reverse, forward</param>
        /// <exception cref="AsterAriException">400 - The provided operation parameter was invalid</exception>
        /// <exception cref="AsterAriException">404 - The playback cannot be found</exception>
        /// <exception cref="AsterAriException">409 - The operation cannot be performed in the playback's current state</exception>
        Task ControlAsync(string playbackId, string operation);
    }
}