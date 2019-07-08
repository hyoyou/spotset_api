using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotSet.Api.Constants;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public class SpotifyService : ISpotifyService
    {
        private IHttpClientFactory _httpFactory;
        private string _spotifyApiKey;
        private string _spotifyApiSecret;

        public SpotifyService(IHttpClientFactory httpFactory, string spotifyApiKey, string spotifyApiSecret)
        {
            _httpFactory = httpFactory;
            _spotifyApiKey = spotifyApiKey;
            _spotifyApiSecret = spotifyApiSecret;
        }

        public async Task<string> GetAccessToken()
        {
            var token = await GetAuthorization();

            return token.access_token;
        }

        private async Task<SpotifyAccessToken> GetAuthorization()
        {
            string credentials = $"{_spotifyApiKey}:{_spotifyApiSecret}";

            var authClient = _httpFactory.CreateClient();
            authClient.DefaultRequestHeaders.Accept.Clear();
            authClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpConstants.AppJson));
            authClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(HttpConstants.Basic, Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

            var requestData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(HttpConstants.GrantType, HttpConstants.ClientCred)
            };

            var requestBody = new FormUrlEncodedContent(requestData);
            var request = await authClient.PostAsync(HttpConstants.SpotifyAuthUri, requestBody);
            var response = await request.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SpotifyAccessToken>(response);
        }
    }
}