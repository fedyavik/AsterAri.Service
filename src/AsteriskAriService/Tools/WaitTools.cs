using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Tools
{
    /// <summary>
    /// Different waiting scenarios
    /// </summary>
    public static class WaitTools
    {
        /// <summary>
        /// Waiting until someone disconnects or moves to another bridge
        /// </summary>
        /// <param name="channels"></param>
        /// <returns>The channel that came out first</returns>
        public static async Task<ChannelAri> WhenAnyChannelLeft(params ChannelAri[] channels)
        {
            using var cts = new CancellationTokenSource();
            var tasks = new List<Task<ChannelAri>>();
            foreach (var channel in channels)
                tasks.Add(channel.WaitLeft(cts.Token));

            var firstCompleteTask = await Task.WhenAny(tasks);
            cts.Cancel();
            return await firstCompleteTask;
        }
        
        /// <summary>
        /// Waiting for the channel to finish listening to the recording
        /// </summary>
        /// <param name="playback"></param>
        /// <param name="channel"></param>
        /// <returns>Returns true if the channel has been listened to, otherwise false</returns>
        public static async Task<bool> WaitChannelListenPlayback (PlaybackAri playback, ChannelAri channel)
        {
            using var cts = new CancellationTokenSource();
            var destroyTask = channel.WaitLeft(cts.Token);
            var playbackTask = playback.WaitFinished(cts.Token);
            var firstComplete = await Task.WhenAny(destroyTask, playbackTask);
            cts.Cancel();
            return firstComplete == playbackTask;
        }
        
        /// <summary>
        /// Wait for the specified time or until the channel is disconnected
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="seconds"></param>
        /// <returns>Returns the channel if it is disconnected, if the timeout has passed, it returns null</returns>
        public static async Task<ChannelAri?> WaitChannelLeftOrTimeout(ChannelAri channel, float seconds)
        {
            using var cts = new CancellationTokenSource();
            var waitTask = WaitTimeout(seconds, cts.Token);
            var clientLeftTask = channel.WaitLeft(cts.Token);
            var firstEndTask = await Task.WhenAny(waitTask, clientLeftTask);
            cts.Cancel();
            return firstEndTask == clientLeftTask ? channel : null;
        }
        
        /// <summary>
        /// Wait for someone to answer the customer's call
        /// </summary>
        /// <param name="client"></param>
        /// <param name="timeout"></param>
        /// <param name="rings"></param>
        /// <returns>Returns the channel that first responded.</returns>
        /// <exception cref="RingTargetsLeftException">If everyone rejects the customer's call</exception>
        /// <exception cref="ClientLeftException">If the client disconnected without waiting for a response</exception>
        /// <exception cref="AriTimeoutException">If the call time is up</exception>
        public static async Task<ChannelAri> WaitRingAnswer(ChannelAri client, float timeout, params ChannelAri[] rings)
        {
            using var cts = new CancellationTokenSource();
            var tasksWaitAnswer = new List<Task<ChannelAri>>();
            foreach (var ring in rings)
                tasksWaitAnswer.Add(ring.WaitAnswer(cts.Token));

            tasksWaitAnswer.Add(WaitTimeout<ChannelAri>(timeout,cts.Token));
            tasksWaitAnswer.Add(WaitAllChannelLeft<ChannelAri>(cts.Token, rings));
            tasksWaitAnswer.Add(client.WaitLeft<ChannelAri>(cts.Token));
            
            var firstEnd =  await Task.WhenAny(tasksWaitAnswer);
            cts.Cancel();
            return await firstEnd;
        }
        
        /// <summary>
        /// We are waiting for everyone to turn off
        /// </summary>
        /// <param name="token"></param>
        /// <param name="channels"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="RingTargetsLeftException"></exception>
        public static async Task<T> WaitAllChannelLeft<T>(CancellationToken token, params ChannelAri[] channels)
        {
            var tasksRingLeft = new List<Task>();
            foreach (var ring in channels)
                tasksRingLeft.Add(ring.WaitLeft(token));
            await Task.WhenAll(tasksRingLeft);
            throw new RingTargetsLeftException(channels.Select(x => x.PhoneNumber));
        }
        
        public static async Task WaitTimeout(float seconds, CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds), token);
            throw new AriTimeoutException();
        }
        public static async Task<T> WaitTimeout<T>(float seconds, CancellationToken token)
        {
            await WaitTimeout(seconds, token);
            throw new AriTimeoutException();
        }
    }
}