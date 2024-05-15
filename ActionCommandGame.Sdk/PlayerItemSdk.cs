using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Net;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class PlayerItemSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlayerItemSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //Find
        public async Task<IList<PlayerItemResult>> Find(int? playerId = null)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/PlayerItem";
            if (playerId.HasValue)
            {
                route = $"/PlayerItem?playerId={playerId.Value}";
            }

            try
            {
                
                var response = httpClient.GetAsync(route).Result;
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return new List<PlayerItemResult>();
                }

                var playerItems = await response.Content.ReadFromJsonAsync<IList<PlayerItemResult>>();

                return playerItems;
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or investigate further
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You may return, throw, or continue depending on the scenario
                return null;
            }

            
        }

        //Get
        public async Task<PlayerItemResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/PlayerItem/{id}";

            var response = httpClient.GetAsync(route).Result;

            response.EnsureSuccessStatusCode();

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<PlayerItemResult>();

           return result;
        }

        //Create
        public async Task<PlayerItemResult?> Create(PlayerItemRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/PlayerItem?playerId={request.PlayerId}&itemId={request.ItemId}";

            var response = httpClient.PostAsJsonAsync(route, request).Result;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PlayerItemResult>();

            return result;
        }

        //Update
        public async Task<PlayerItemResult?> Update(int id, PlayerItemRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/PlayerItem/{id}";

            var response = await httpClient.PutAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PlayerItemResult>();

            return result;
        }

        //Delete
        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/PlayerItem/{id}";

            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
