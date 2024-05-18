using System.Net.Http.Json;
using ActionCommandGame.Security.Model;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Identity;


namespace ActionCommandGame.Sdk
{
    public class IdentitySdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentitySdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<JwtAuthenticationResult> SignIn(UserSignInRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/api/Identity/sign-in";

            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JwtAuthenticationResult>();
            if (result is null)
            {
                return new JwtAuthenticationResult()
                {
                    Messages = new List<ServiceMessage>()
                    {
                        new ServiceMessage { Code = "ApiError", Message = "An Api error occurred" }
                    }
                };
            }

            return result;
        }

        public async Task<JwtAuthenticationResult> Register(UserRegisterRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/api/Identity/register";

            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JwtAuthenticationResult>();
            if (result is null)
            {
                return new JwtAuthenticationResult()
                {
                    Messages = new List<ServiceMessage>()
                    {
                        new ServiceMessage { Code = "ApiError", Message = "An Api error occurred" }
                    }
                };
            }

            return result;
        }

        public async Task<JwtAuthenticationResult> SetUserAsUser(string userName)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/Identity/SetUserAsUser?userName={userName}";

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JwtAuthenticationResult>();
            if (result is null)
            {
                return new JwtAuthenticationResult()
                {
                    Messages = new List<ServiceMessage>()
                    {
                        new ServiceMessage { Code = "ApiError", Message = "An Api error occurred" }
                    }
                };
            }

            return result;
        }

        public async Task<JwtAuthenticationResult> SetUserAsAdmin(string userName)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/api/Identity/SetUserAsAdmin";

            var response = await httpClient.PostAsJsonAsync(route, userName);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JwtAuthenticationResult>();
            if (result is null)
            {
                return new JwtAuthenticationResult()
                {
                    Messages = new List<ServiceMessage>()
                    {
                        new ServiceMessage { Code = "ApiError", Message = "An Api error occurred" }
                    }
                };
            }

            return result;
        }

        public async Task<IdentityUser> GetIdentityUserFromId(string userId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            
            var route = $"/api/Identity/GetIdentityUserFromName?userId={userId}";

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IdentityUser>();
            

            return result;
        }
    }
}
