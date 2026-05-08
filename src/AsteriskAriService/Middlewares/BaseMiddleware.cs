namespace AsteriskAriService.Middlewares
{
    public abstract class BaseAriMiddleware
    {
        public abstract Task InvokeAsync(Func<Task> next);
    }
}