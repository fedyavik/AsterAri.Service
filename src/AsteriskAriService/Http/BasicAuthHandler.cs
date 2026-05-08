using System.Net.Http.Headers;
using System.Text;

namespace AsteriskAriService.Http
{
    internal class BasicAuthHandler : DelegatingHandler
    {
        private readonly string _encodedCredentials;

        public BasicAuthHandler(string username, string password)
        {
            var bytes = Encoding.ASCII.GetBytes($"{username}:{password}");
            _encodedCredentials = Convert.ToBase64String(bytes);
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", _encodedCredentials);

            return base.SendAsync(request, cancellationToken);
        }
    }
}