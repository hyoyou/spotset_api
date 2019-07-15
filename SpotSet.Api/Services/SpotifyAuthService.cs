using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotSet.Api.Constants;
using SpotSet.Api.Exceptions;
using SpotSet.Api.Models;

namespace SpotSet.Api.Services
{
    public class SpotifyAuthService : ISpotifyAuthService
    {
        private IHttpClientFactory _httpFactory;
        private readonly IConfiguration _configuration;

        public SpotifyAuthService(IHttpClientFactory httpFactory, IConfiguration configuration)
        {
            _httpFactory = httpFactory;
            _configuration = configuration;
        }

        public async Task<string> GetAccessToken()
        {
            try
            {
                var token = await GetAuthorization();
                if (token?.access_token == null)
                {
                    throw new SpotifyAuthException("There was an error with authenticating the app.");
                }

                return token.access_token;
            } 
            catch (SpotifyAuthException ex)
            {
                throw ex;
            }
        }

        private async Task<SpotifyAccessToken> GetAuthorization()
        {
            var apiKey = _configuration["SpotifyApiKey"];
            var apiSecret = _configuration["SpotifyApiSecret"];
            string credentials = $"{apiKey}:{apiSecret}";

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