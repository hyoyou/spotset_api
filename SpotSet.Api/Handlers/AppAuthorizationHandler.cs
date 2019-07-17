using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SpotSet.Api.Constants;
using SpotSet.Api.Services;

namespace SpotSet.Api.Handlers
{
    public class AppAuthorizationHandler : DelegatingHandler
    {
        private ISpotifyAuthService _authService;
        private static IHttpClientFactory _httpFactory;
        private readonly IConfiguration _configuration;

        public AppAuthorizationHandler(IConfiguration configuration, IHttpClientFactory httpFactory)
        {
            _configuration = configuration;
            _httpFactory = httpFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _authService = new SpotifyAuthService(_httpFactory, _configuration);
            var token = await _authService.GetAccessToken();
            request.Headers.Authorization = new AuthenticationHeaderValue(ApiConstants.Bearer, token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}