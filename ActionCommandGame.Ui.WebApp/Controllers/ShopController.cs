using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Ui.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ActionCommandGame.Ui.WebApp.Controllers
{
    public class ShopController : Controller
    {
        
        private readonly GameSdk _gameSdk;
        private readonly ItemSdk _itemSdk;
        private readonly PlayerSdk _playerSdk;
        private readonly PlayerItemSdk _playerItemSdk;

        public ShopController(GameSdk gameSdk, ItemSdk itemSdk, PlayerSdk playerSdk, PlayerItemSdk playerItemSdk)
        {
            _gameSdk = gameSdk;
            _itemSdk = itemSdk;
            _playerSdk = playerSdk;
            _playerItemSdk = playerItemSdk;
        }
        public async Task<IActionResult> Index(string? gameResultJson, int playerId)
        {
            GameResult gameResult = null;
            var itemList = await _itemSdk.Find();

            if(string.IsNullOrEmpty(gameResultJson))
            {
                gameResult = new GameResult{ Player = await _playerSdk.Get(playerId)};
            }
            else
            {
                gameResult = JsonSerializer.Deserialize<GameResult>(gameResultJson);
            }

            GameOverview gameOverview = new GameOverview { GameResult=gameResult, Items = itemList, PlayerItems = await _playerItemSdk.Find()};

            return View(gameOverview);
        }

        public async Task<IActionResult> Buy(int ItemId, int playerId)
        {
            GameResult gameResult = await _gameSdk.Buy(playerId, ItemId);

            string gameResultJson = JsonSerializer.Serialize(gameResult);

            return RedirectToAction("Index", new { gameResultJson = gameResultJson });
        }
    }
}
