using System.Reactive.Disposables;
using AsteriskAriService.Extensions;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Asterisk.Event;
using AsteriskAriService.Websocket;

namespace AsteriskAriService.Models.Ari
{
    /// <summary>
    /// Limited by scope lifetime
    /// </summary>
    /// <param name="wsAriClient"></param>
    public class PlaybackAri (IWsAriClient wsAriClient) : PlaybackModel, IDisposable
    {
        private TaskCompletionSource<bool> _isFinishedAsyncStatus = new ();
        private CompositeDisposable _compositeDisposable = new();
        private bool _isDisposed;

        internal void Init(PlaybackModel model)
        {
            Id = model.Id;
            Media_uri = model.Media_uri;
            Next_media_uri = model.Next_media_uri;
            Target_uri = model.Target_uri;
            Language = model.Language;
            State = model.State;
            wsAriClient.OnPlaybackFinished
                .Subscribe(AsteriskPlaybackFinished)
                .AddTo(_compositeDisposable);
        }
        
        /// <summary>
        /// Waiting for audio playback to finish
        /// </summary>
        /// <param name="token"></param>
        public async Task WaitFinished(CancellationToken token)
        {
            await _isFinishedAsyncStatus.Task.WaitAsync(token);
        }
        
        /// <summary>
        /// Asterisk reports that some kind of playback has ended
        /// </summary>
        /// <param name="e"></param>
        private void AsteriskPlaybackFinished(PlaybackFinishedEvent e)
        {
            if (e.Playback.Id != Id)
                return;
            if (e.Playback.State == "done")
            {
                _isFinishedAsyncStatus.TrySetResult(true);
                Dispose();                
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            _compositeDisposable.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}