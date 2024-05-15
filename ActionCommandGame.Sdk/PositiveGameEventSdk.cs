using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class PositiveGameEventSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PositiveGameEventSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //Find
        public async Task<IList<PositiveGameEventResult>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/PositiveGameEvent";

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var positiveGameEvents = await response.Content.ReadFromJsonAsync<IList<PositiveGameEventResult>>();

            if (positiveGameEvents is null)
            {
                return new List<PositiveGameEventResult>();
            }

            return positiveGameEvents;
        }

        //Get
        public async Task<PositiveGameEventResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/PositiveGameEvent/{id}";

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PositiveGameEventResult>();

            return result;
        }

        //Get
        public async Task<PositiveGameEventResult?> GetRandomPositiveGameEvent(bool hasAttack)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/PositiveGameEvent/random/{hasAttack}";

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PositiveGameEventResult>();

            return result;
        }

        //Create
        public async Task<PositiveGameEventResult?> Create(PositiveGameEventRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/PositiveGameEvent";

            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PositiveGameEventResult>();

            return result;
        }

        //Update
        public async Task<PositiveGameEventResult?> Update(int id, PositiveGameEventRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/PositiveGameEvent/{id}";

            var response = await httpClient.PutAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PositiveGameEventResult>();

            return result;
        }

        

        //Delete
        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/PositiveGameEvent/{id}";

            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
