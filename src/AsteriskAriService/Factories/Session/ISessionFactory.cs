using AsteriskAriService.Models.Ari;

namespace AsteriskAriService.Factories.Session
{
    public interface ISessionFactory
    {
        /// <summary>
        /// create a session for the channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public Task<ClientSessionAri> CreateSession(string channelId);
    }
}