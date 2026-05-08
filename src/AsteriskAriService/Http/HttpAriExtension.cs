using AsteriskAriService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AsteriskAriService.Http
{
    public static class HttpAriExtension
    {
        public static IHttpClientBuilder AddAriHttpClient<TClient, TImpl>(
            this IServiceCollection services,
            AriConnectOptions options)
            where TClient : class
            where TImpl : class, TClient
        {
            var scheme = options.UseSsl ? "https://" : "http://";
            return services.AddHttpClient<TClient, TImpl>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri($"{scheme}{options.Hostname}:{options.Port}/ARI/");
                    client.Timeout = TimeSpan.FromSeconds(5);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(10))
                .AddHttpMessageHandler(() =>
                    new BasicAuthHandler(options.UserName, options.Password));
        }
    }
}