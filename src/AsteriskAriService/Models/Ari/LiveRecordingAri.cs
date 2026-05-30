using AsteriskAriService.Actions.Recording;
using AsteriskAriService.Extensions;
using AsteriskAriService.Models.Asterisk;
using Microsoft.Extensions.Logging;

namespace AsteriskAriService.Models.Ari
{
    /// <summary>
    /// Limited by scope lifetime
    /// </summary>
    /// <param name="recordingsActions"></param>
    /// <param name="logger"></param>
    public class LiveRecordingAri(
        IRecordingsAriActions recordingsActions,
        ILogger<LiveRecordingAri> logger
        ) : LiveRecordingModel, IDisposable, IAsyncDisposable
    {
        public string RecordingName { get; private set; }
        private DateTime _createdAt;
        private bool _isStopped;

        internal void Init(LiveRecordingModel model)
        {
            Name = model.Name;
            Format = model.Format;
            Target_uri = model.Target_uri;
            State = model.State;
            Duration = model.Duration;
            Talking_duration  = model.Talking_duration;
            Silence_duration = model.Silence_duration;
            Cause = model.Cause;
            
            _createdAt = DateTime.Now;
            RecordingName = $"{Name}.{Format}";
        }
        public async Task Stop()
        {
            if (_isStopped)
                return;
            _isStopped = true;
            await recordingsActions.TryStopAsync(Name);
        }
        public async Task Pause()
        {
            await recordingsActions.PauseAsync(Name);
        }
        public async Task UnPause()
        {
            await recordingsActions.UnpauseAsync(Name);
        }
        public async Task Mute()
        {
            await recordingsActions.MuteAsync(Name);
        }
        public async Task UnMute()
        {
            await recordingsActions.UnmuteAsync(Name);
        }

        public void Dispose()
        {
            if (_isStopped)
                return;
            Stop().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }
        public async ValueTask DisposeAsync()
        {
            if (_isStopped)
                return;
            await Stop();
            GC.SuppressFinalize(this);
        }
    }
}