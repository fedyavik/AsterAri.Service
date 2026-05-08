using AsteriskAriService.Factories.Session;
using AsteriskAriService.Models;
using AsteriskAriService.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AsteriskAriService.Middlewares
{
    public class SessionPipeline(
        IServiceProvider serviceProvider,
        IOptions<ServerAriOptions> ariOptions,
        ISessionFactory sessionFactory)
    {
        public async Task Start(string channelId, List<string> args, SessionStorage? redirectData = null)
        {
            var clientSessionAri = await sessionFactory.CreateSession(channelId);
            clientSessionAri.Parameters = args;
            if (redirectData is not null)
                clientSessionAri.CallItems = redirectData;
            await NextMiddleware(0);
        }
        private async Task NextMiddleware(int middlewareIndex)
        {
            var middlewareType = ariOptions.Value.Middlewares[middlewareIndex];
            var middleware = (BaseAriMiddleware)serviceProvider.GetRequiredService(middlewareType);
            await middleware.InvokeAsync(async () => await NextMiddleware(middlewareIndex + 1));
        }
    }
}