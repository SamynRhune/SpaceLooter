using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Net.Http.Json;
namespace ActionCommandGame.Sdk
{
    public class AccountSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AccountResult?> GetAccount(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Account/{id}";

            //https://localhost:7128/Player/1
            //https://localhost:7128/player/1

            var response = httpClient.GetAsync(route).Result;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AccountResult>();

            return result;
        }

        public async Task<AccountResult?> UpdateAccount(int id, AccountRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Account/{id}";

            var response = await httpClient.PutAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AccountResult>();

            return result;
        }

        public async Task Delete(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Account?userId={id}";
            //https://localhost:7128/Account?userId=d98d254e-ad69-40bb-ac56-72706fcf97fe

            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }

    }
}
