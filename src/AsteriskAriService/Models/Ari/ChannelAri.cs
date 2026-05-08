using System.Reactive.Disposables;
using AsteriskAriService.Actions.Channel;
using AsteriskAriService.Extensions;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Asterisk.Event;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Websocket;

namespace AsteriskAriService.Models.Ari
{
    /// <summary>
    /// Limited by scope lifetime
    /// </summary>
    /// <param name="wsAriClient"></param>
    /// <param name="channelAriActions"></param>
    public class ChannelAri (IWsAriClient wsAriClient,
        IChannelAriActions channelAriActions)
        : ChannelModel, IDisposable
    {
        public string PhoneNumber { get; private set; }
        private TaskCompletionSource<bool> _isDisposedAsyncStatus = new ();
        private TaskCompletionSource<bool> _isLeftAsyncStatus = new ();
        private TaskCompletionSource<bool> _isAnswerAsyncStatus = new ();
        private bool _isAnswered;
        private bool _isDisposed;
        /// <summary>
        /// The channel should be cleared soon.
        /// </summary>
        private bool _isWaitDispose;
        private CompositeDisposable _hangupDisposable = new();

        internal void Init(ChannelModel model, string number)
        {
            Id = model.Id;
            Name = model.Name;
            State = model.State;
            Caller = model.Caller;
            Connected = model.Connected;
            Accountcode = model.Accountcode;
            Dialplan = model.Dialplan;
            CreationTime = model.CreationTime;
            Language = model.Language;
            
            PhoneNumber = number;
            wsAriClient.OnChannelStateChange.Subscribe(AsteriskUpdateState).AddTo(_hangupDisposable);
            wsAriClient.OnChannelDestroyed.Subscribe(AsteriskDestroyChannel).AddTo(_hangupDisposable);
            wsAriClient.OnChannelLeftBridge.Subscribe(LeftBridge).AddTo(_hangupDisposable);
        }

        /// <summary>
        /// Call the line
        /// </summary>
        public async Task Call(string channelId, int timeout = 30)
        {
            await channelAriActions.DialAsync(Id, channelId, timeout);
        }
        
        /// <summary>
        /// Switch the channel to the response state
        /// </summary>
        public async Task Answer()
        {
            if (_isAnswered)
                return;
            await channelAriActions.AnswerAsync(Id);
            _isAnswered = true;
        }
        
        /// <summary>
        /// This will not end the asterisk channel.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            _hangupDisposable.Dispose();
            _isDisposedAsyncStatus.SetResult(true);
            _isLeftAsyncStatus.SetResult(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Set the parameter
        /// </summary>
        public async Task SetVariable(string varName, string varValue)
        {
            await channelAriActions.SetChannelVarAsync(Id,varName, varValue);
        }

        /// <summary>
        /// We are waiting until the channel is turned off
        /// </summary>
        /// <returns></returns>
        public async Task<T> WaitDestroy<T>(CancellationToken token)
        {
            await _isDisposedAsyncStatus.Task.WaitAsync(token);
            throw new ClientLeftException(this);
        }
        
        /// <summary>
        /// We are waiting until the channel is disconnected or transferred to another bridge
        /// </summary>
        /// <returns></returns>
        public async Task<T> WaitLeft<T>(CancellationToken token)
        {
            await WaitLeft(token);
            throw new ClientLeftException(this);
        }
        
        /// <summary>
        /// We are waiting until the channel is disconnected or transferred to another bridge
        /// </summary>
        /// <returns></returns>
        public async Task<ChannelAri> WaitLeft(CancellationToken token)
        {
            var destroyTask = _isDisposedAsyncStatus.Task.WaitAsync(token);
            var bridgeLeftTask = _isLeftAsyncStatus.Task.WaitAsync(token);
            await Task.WhenAny(destroyTask, bridgeLeftTask);
            return this;
        }
        
        /// <summary>
        /// We are waiting until the channel is switched to the response state
        /// </summary>
        /// <returns></returns>
        public async Task<ChannelAri> WaitAnswer(CancellationToken token)
        {
            await _isAnswerAsyncStatus.Task.WaitAsync(token);
            return this;
        }
        
        /// <summary>
        /// When redirecting, the current channel will be cleared when the new one is connected to some bridge.
        /// </summary>
        public void DisposeWhenLeft()
        {
            _isWaitDispose = true;
        }
        /// <summary>
        /// Asterisk reports that the channel status has changed
        /// </summary>
        /// <param name="e"></param>
        private void AsteriskUpdateState(ChannelStateChangeEvent e)
        {
            if (e.Channel.Id != Id)
                return;
            if (e.Channel.State == "Up")
                _isAnswerAsyncStatus.SetResult(true);
            State = e.Channel.State;
        }
        
        /// <summary>
        /// The channel is disconnected from the bridge
        /// </summary>
        private void LeftBridge(ChannelLeftBridgeEvent e)
        {
            if (e.Channel.Id != Id)
                return;
            if (_isWaitDispose)
            {
                Dispose();
                return;
            }
            SetLeftStatus();
        }

        private void SetLeftStatus()
        {
            var tcs = _isLeftAsyncStatus;
            _isLeftAsyncStatus = new TaskCompletionSource<bool>();
            tcs.SetResult(true);
        }
        /// <summary>
        /// Asterisk reports that the channel has been deleted
        /// </summary>
        /// <param name="e"></param>
        private void AsteriskDestroyChannel(ChannelDestroyedEvent e)
        {
            if (e.Channel.Id != Id)
                return;
            Dispose();
        }
    }
}