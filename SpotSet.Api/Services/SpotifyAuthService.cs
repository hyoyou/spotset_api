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
                var token = await TokenRequest();
                if (token?.access_token == null)
                {
                    throw new SpotifyAuthException(ErrorConstants.SpotifyAuthError);
                }

                return token.access_token;
            } 
            catch (SpotifyAuthException ex)
            {
                throw ex;
            }
        }

        private async Task<SpotifyAccessToken> TokenRequest()
        {
            var authClient = CreateRequest(out var requestBody);
            var request = await SendRequest(authClient, requestBody);
            return await ProcessResponse(request);
        }

        private HttpClient CreateRequest(out FormUrlEncodedContent requestBody)
        {
            var apiKey = _configuration[ApiConstants.SpotifyApiKey];
            var apiSecret = _configuration[ApiConstants.SpotifyApiSecret];
            string credentials = $"{apiKey}:{apiSecret}";

            var authClient = _httpFactory.CreateClient();
            authClient.DefaultRequestHeaders.Accept.Clear();
            authClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.AppJson));
            authClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ApiConstants.Basic,
                Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

            var requestData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(ApiConstants.GrantType, ApiConstants.ClientCred)
            };

            requestBody = new FormUrlEncodedContent(requestData);
            return authClient;
        }

        private static async Task<HttpResponseMessage> SendRequest(HttpClient authClient, FormUrlEncodedContent requestBody)
        {
            var request = await authClient.PostAsync(ApiConstants.SpotifyAuthUri, requestBody);
            return request;
        }

        private static async Task<SpotifyAccessToken> ProcessResponse(HttpResponseMessage request)
        {
            var response = await request.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SpotifyAccessToken>(response);
        }
    }
}