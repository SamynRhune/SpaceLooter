using ActionCommandGame.Services.Model;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Net;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class NegativeGameEventSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NegativeGameEventSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //Find
        public async Task<IList<NegativeGameEventResult>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/NegativeGameEvent";

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var negativeGameEvents = await response.Content.ReadFromJsonAsync<IList<NegativeGameEventResult>>();

            if (negativeGameEvents is null)
            {
                return new List<NegativeGameEventResult>();
            }

            return negativeGameEvents;
        }

        //Get
        public async Task<NegativeGameEventResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/NegativeGameEvent/{id}";

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<NegativeGameEventResult>();

            return result;
        }

        //Get
        public async Task<NegativeGameEventResult?> GetRandomNegativeGameEvent()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/NegativeGameEvent/random";

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<NegativeGameEventResult>();
            return result;
            
        }

            //Create
            public async Task<NegativeGameEventResult?> Create(NegativeGameEventRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/NegativeGameEvent";

            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<NegativeGameEventResult>();

            return result;
        }

        //Update
        public async Task<NegativeGameEventResult?> Update(int id, NegativeGameEventRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/NegativeGameEvent/{id}";

            var response = await httpClient.PutAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<NegativeGameEventResult>();

            return result;
        }

        //Delete
        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/NegativeGameEvent/{id}";

            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
