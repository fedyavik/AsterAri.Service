using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Actions.Recording
{
    public interface IRecordingsAriActions
    {
        /// <summary>
        /// List recordings that are complete.
        /// </summary>
        /// <returns></returns>
        Task<List<StoredRecordingModel>> ListStoredAsync();
        
        /// <summary>
        /// Get a stored recording's details.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <returns></returns>
        Task<StoredRecordingModel> GetStoredAsync(string recordingName);
        
        /// <inheritdoc cref="GetStoredAsync"/>>
        Task<StoredRecordingModel?> TryGetStoredAsync(string recordingName);
        
        /// <summary>
        /// Delete a stored recording.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <returns></returns>
        Task DeleteStoredAsync(string recordingName);
        
        /// <summary>
        /// Get the file associated with the stored recording.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <exception cref="AsterAriException">403 - The recording file could not be opened</exception>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <returns></returns>
        Task<byte[]> GetStoredFileAsync(string recordingName);
        
        /// <summary>
        /// Copy a stored recording.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <param name="destinationRecordingName"></param>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <exception cref="AsterAriException">409 - A recording with the same name already exists on the system</exception>
        /// <returns></returns>
        Task<StoredRecordingModel> CopyStoredAsync(string recordingName, string destinationRecordingName);
        
        /// <summary>
        /// List live recordings.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <returns></returns>
        Task<LiveRecordingModel> GetLiveAsync(string recordingName);
        
        /// <summary>
        /// Stop a live recording and discard it.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <returns></returns>
        Task CancelAsync(string recordingName);
        
        /// <summary>
        /// Stop a live recording and store it.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <returns></returns>
        Task StopAsync(string recordingName);
        
        /// <inheritdoc cref="StopAsync"/>>
        Task TryStopAsync(string recordingName);
        
        /// <summary>
        /// Pause a live recording.
        /// Pausing a recording suspends silence detection, which will be restarted when the recording is unpaused.
        /// Paused time is not included in the accounting for maxDurationSeconds.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <exception cref="AsterAriException">409 - Recording not in session</exception>
        /// <returns></returns>
        Task PauseAsync(string recordingName);
        
        /// <summary>
        /// Unpause a live recording.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <exception cref="AsterAriException">409 - Recording not in session</exception>
        /// <returns></returns>
        Task UnpauseAsync(string recordingName);
        
        /// <summary>
        /// Mute a live recording.
        /// Muting a recording suspends silence detection, which will be restarted when the recording is unmuted.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <exception cref="AsterAriException">409 - Recording not in session</exception>
        /// <returns></returns>
        Task MuteAsync(string recordingName);
        
        /// <summary>
        /// Unmute a live recording.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <exception cref="AsterAriException">404 - Recording not found</exception>
        /// <exception cref="AsterAriException">409 - Recording not in session</exception>
        /// <returns></returns>
        Task UnmuteAsync(string recordingName);
    }
}