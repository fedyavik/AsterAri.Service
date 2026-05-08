using System.Collections.Concurrent;
using AsteriskAriService.Models.Ari;

namespace AsteriskAriService.Session
{
    public class SessionManager: ISessionManager
    {
        private readonly ConcurrentDictionary<string,ClientSessionAri> _sessions = new();
        
        public void AddSession(ClientSessionAri clientSessionAri)
        {
            _sessions[clientSessionAri.Initiator.Id] = clientSessionAri;
        }

        public void RemoveSession(ClientSessionAri clientSessionAri)
        {
            _sessions.Remove(clientSessionAri.Initiator.Id, out var _);
        }

        public ClientSessionAri? GetSession(string channelId)
        {
            _sessions.TryGetValue(channelId, out var session);
            return session;
        }
    }
}