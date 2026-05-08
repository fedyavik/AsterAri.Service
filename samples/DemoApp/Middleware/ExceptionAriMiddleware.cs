using AsteriskAriService.Middlewares;
using AsteriskAriService.Models.Ari;
using AsteriskAriService.Models.Exceptions;

namespace DemoApp.Middleware
{
    public class ExceptionAriMiddleware(
        ILogger<ExceptionAriMiddleware> logger,
        ClientSessionAri clientSessionAri) : BaseAriMiddleware
    {
        /// <inheritdoc />
        public override async Task InvokeAsync(Func<Task> next)
        {
            try
            {
                await next();
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        /// <summary>
        /// Writing down the reason for the interruption of call processing
        /// </summary>
        /// <param name="exception"></param>
        private void LogError(Exception exception)
        {
            var src = clientSessionAri.Initiator;
            switch (exception)
            {
                case ClientLeftException:
                    logger.LogInformation("Call Processing {Number}, interrupted at the initiative of the client",
                        src.PhoneNumber);
                    break;
                case AriTimeoutException:
                    logger.LogInformation("Call Processing {Number}, aborted by timeout",
                        src.PhoneNumber);
                    break;
                case AsterAriException:
                    logger.LogWarning("Call processing aborted {Number}: {Error}",
                        src.PhoneNumber, exception.Message);
                    break;
                default:
                    logger.LogError("An error occurred while processing the call {Number}: {Error} \n {Trace}",
                        src.PhoneNumber, exception.Message, exception.StackTrace);
                    break;
            }
        }
    }
}