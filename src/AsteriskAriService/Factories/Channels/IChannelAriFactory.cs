using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Models.Number;

namespace AsteriskAriService.Factories.Channels
{
    public interface IChannelAriFactory
    {
        /// <summary>
        /// Create a new channel (originate).
        /// The new channel is created immediately and a snapshot of it returned.
        /// If a Stasis application is provided it will be automatically subscribed to the originated channel for further events and updates.
        /// </summary>
        /// <param name="endpoint">Endpoint to cal</param>
        /// <param name="originator">The unique id of the channel which is originating this one</param>
        /// <returns></returns>
        public Task<ChannelAri?> TryCreateAsync(AriNumber endpoint, string? originator = null);
        
        /// <summary>
        /// Channel details.
        /// </summary>
        /// <param name="channelId"></param>
        /// <exception cref="AsterAriException">404 - Channel not found</exception>
        /// <returns></returns>
        Task<ChannelAri> GetAsync(string channelId);
    }
}