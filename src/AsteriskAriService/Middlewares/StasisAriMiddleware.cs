using AsteriskAriService.Models;
using AsteriskAriService.Models.Ari;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AsteriskAriService.Middlewares
{
    internal class StasisAriMiddleware(
        ILogger<StasisAriMiddleware> logger,
        IServiceProvider serviceProvider,
        IOptions<ServerAriOptions> options,
        ClientSessionAri clientSessionAri) : BaseAriMiddleware
    {
        public override async Task InvokeAsync(Func<Task> _)
        {
            var stasisHandlers = options.Value.StasisHandlers;
            var action = clientSessionAri.Parameters[0];
            if (!stasisHandlers.TryGetValue(action, out var handler))
            {
                logger.LogWarning("Stasis action not found: {Action}", action);
                return;
            }
            var stasis = (StasisHandler)serviceProvider.GetRequiredService(handler);
            await stasis.Handler();
        }
    }
}