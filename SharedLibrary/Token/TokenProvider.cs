using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Token
{
    public class TokenProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public TokenProvider(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }


        public async Task<string> GetTokenAsync()
        {
            var request = new
            {
                ClientId = _config["ServiceAuth:ClientId"],
                ClientSecret = _config["ServiceAuth:ClientSecret"]
            };

            //this line will call the auth service (microservice)
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5046/auth/token", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                return result.AccessToken;
            }

            throw new Exception("Unable to fetch token");
        }

    }
}
