using ActionCommandGame.Services.Model;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Net;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class GameSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GameSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //Find
        public async Task<ServiceResult<GameResult>> PerformAction(int playerId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Game/{playerId}";

            var response = httpClient.GetAsync(route).Result;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<GameResult>>();
            /*var gameResult = result.Data;*/

            return result;
        }

        //Get
        public async Task<GameResult> Buy(int playerId, int playerItemId)
        {
            //Buy?playerId=1006&itemId=1
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Game/Buy?playerId={playerId}&itemId={playerItemId}";

            var response = httpClient.GetAsync(route).Result;

            response.EnsureSuccessStatusCode();

            /*if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }*/

            var result = response.Content.ReadFromJsonAsync<ServiceResult<GameResult>>().Result;
            var gameResult = result.Data;

            return gameResult;
        }

        //Create
        public async Task<GameResult> ActivateItemId(int playerId, int playerItemId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/Game/Activate?playerId{playerId}&playerItemId{playerItemId}";

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GameResult>();

            return result;
        }

        
    }
}
