using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Net;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class PlayerSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlayerSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //Find
        public async Task<IList<PlayerResult>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/Player";

            var response = httpClient.GetAsync(route).Result;

            response.EnsureSuccessStatusCode();

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            var players = await response.Content.ReadFromJsonAsync<IList<PlayerResult>>();

            if (players is null)
            {
                return new List<PlayerResult>();
            }

            return players;
        }

        //Get
        public async Task<PlayerResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Player/by-id/{id}";

            //https://localhost:7128/Player/1
            //https://localhost:7128/player/1

            var response = httpClient.GetAsync(route).Result;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PlayerResult>();

            return result;
        }

        public async Task<PlayerResult?> GetIdentityId(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Player/by-identity/{id}";

            //https://localhost:7128/Player/1
            //https://localhost:7128/player/1

            var response = httpClient.GetAsync(route).Result;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PlayerResult>();

            return result;
        }

        //Create
        public async Task<PlayerResult?> Create(PlayerRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/Player";

            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PlayerResult>();

            return result;
        }

        //Update
        public async Task<PlayerResult?> Update(int id, PlayerRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Player/{id}";

            var response = await httpClient.PutAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PlayerResult>();

            return result;
        }

        //Delete
        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Player/{id}";

            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }

        
    }
}
