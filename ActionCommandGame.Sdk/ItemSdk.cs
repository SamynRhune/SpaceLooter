using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class ItemSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ItemSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //Find
        public async Task<IList<ItemResult>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/Item";

            var response = httpClient.GetAsync(route).Result;

            response.EnsureSuccessStatusCode();

            var items = await response.Content.ReadFromJsonAsync<IList<ItemResult>>();

            if (items is null)
            {
                return new List<ItemResult>();
            }

            return items;
        }

        //Get
        public async Task<ItemResult?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Item/{id}";

            var response = httpClient.GetAsync(route).Result;

            response.EnsureSuccessStatusCode();

            /*if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }*/

            var result = response.Content.ReadFromJsonAsync<ItemResult>().Result;

            return result;
        }

        //Create
        public async Task<ItemResult?> Create(ItemRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/Item";

            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ItemResult>();

            return result;
        }

        //Update
        public async Task<ItemResult?> Update(int id, ItemRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Item/{id}";

            var response = await httpClient.PutAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ItemResult>();

            return result;
        }

        //Delete
        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Item/{id}";

            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
