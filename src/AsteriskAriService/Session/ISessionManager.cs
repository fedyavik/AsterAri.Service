using AsteriskAriService.Models.Ari;

namespace AsteriskAriService.Session
{
    public interface ISessionManager
    {
        /// <summary>
        /// Adding a session
        /// </summary>
        /// <param name="clientSessionAri"></param>
        public void AddSession(ClientSessionAri clientSessionAri);

        /// <summary>
        /// Deleting a session
        /// </summary>
        /// <param name="clientSessionAri"></param>
        public void RemoveSession(ClientSessionAri clientSessionAri);

        /// <summary>
        /// Getting sessions by ID
        /// </summary>
        /// <param name="channelId"></param>
        public ClientSessionAri? GetSession(string channelId);
    }
}