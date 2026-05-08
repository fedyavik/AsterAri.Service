using System.Reactive.Disposables;
using AsteriskAriService.Extensions;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Asterisk.Event;
using AsteriskAriService.Websocket;

namespace AsteriskAriService.DtmfHandlers
{
    public class BaseDtmfHandler (IWsAriClient wsAriClient) : IDisposable
    {
        protected Dictionary<string, List<char>> Buffers { get; } = new();
        protected readonly CompositeDisposable DtmfDisposable = new();
        protected ChannelAri[] ListenChannels = [];
        private bool _isDisposed;
        public async Task Start(params ChannelAri[] listen)
        {
            ListenChannels = listen;
            wsAriClient.OnChannelDtmfReceived.Subscribe(DtmfReceived).AddTo(DtmfDisposable);
        }
        
        /// <summary>
        /// Asterisk reports that a dtmf has been received
        /// </summary>
        /// <param name="e"></param>
        private void DtmfReceived(ChannelDtmfReceivedEvent e)
        {
            foreach (var channel in ListenChannels)
            {
                if (channel.Id != e.Channel.Id)
                    continue;
                GetDigit(channel, e.Digit);
                return;
            }
        }

        protected virtual void GetDigit(ChannelAri channelAri, char digit)
        {
            var channelBuffer = GetChannelBuffer(channelAri);
            switch (digit)
            {
                case '*':
                    if (channelBuffer.Count > 0)
                    {
                        var sequence = string.Join("", channelBuffer);
                        CodeSequenceHandler(channelAri, sequence);
                    }
                    channelBuffer.Clear();
                    break;
                default:
                    channelBuffer.Add(digit);
                    break;
            }
        }

        protected List<char> GetChannelBuffer(ChannelAri channelAri)
        {
            if (Buffers.TryGetValue(channelAri.Id, out var buffer))
                return buffer;
            buffer = new List<char>() { Capacity = 10 };
            Buffers.TryAdd(channelAri.Id, buffer);
            return buffer;
        }
        /// <summary>
        /// The sequence is considered completed when pressed *
        /// </summary>
        /// <param name="channel">the channel that typed the sequence</param>
        /// <param name="sequence"></param>
        protected virtual void CodeSequenceHandler(ChannelAri channel, string sequence)
        {
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            DtmfDisposable.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}