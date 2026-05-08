using AsteriskAriService.Factories.Channels;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Session;

namespace AsteriskAriService.Factories.Session
{
    public class SessionFactory(ISessionManager sessionManager,
        IChannelAriFactory channelAriFactory,
        ClientSessionAri clientSessionAri
    ): ISessionFactory, IDisposable
    {
        public async Task<ClientSessionAri> CreateSession(string channelId)
        {
            var channelAri = await channelAriFactory.GetAsync(channelId);
            clientSessionAri.Initiator = channelAri;
            sessionManager.AddSession(clientSessionAri);
            return clientSessionAri;
        }
        public void Dispose()
        {
            sessionManager.RemoveSession(clientSessionAri);
            GC.SuppressFinalize(this);
        }
    }
}