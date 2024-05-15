using Microsoft.AspNetCore.Mvc;
using ActionCommandGame.Model;
using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Ui.WebApp.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Ui.WebApp.Controllers
{
    public class GameController : Controller
    {

        
        private readonly PlayerSdk _playerSdk;
        private readonly GameSdk _gameSdk;
        private readonly PlayerItemSdk _playerItemSdk;
        private readonly ItemSdk _itemSdk;
        private readonly PositiveGameEventSdk _positiveGameEventSdk;
        private readonly NegativeGameEventSdk _negativeGameEventSdk;
        

        public GameController(PlayerSdk playerSdk, GameSdk gameSdk, PositiveGameEventSdk positiveGameEventSdk,NegativeGameEventSdk negativeGameEventSdk, PlayerItemSdk playerItemSdk, ItemSdk itemSdk)
        {
            
            _playerSdk = playerSdk;
            _gameSdk = gameSdk;
            _positiveGameEventSdk = positiveGameEventSdk;
            _negativeGameEventSdk = negativeGameEventSdk;
            _playerItemSdk = playerItemSdk;
            _itemSdk = itemSdk;
        }
        public async Task<IActionResult> Index(string? serviceResultJson)
        {

            
            /*int playerId = _id;*/
            var currentPlayer = await _playerSdk.GetIdentityId(User.Identity.Name);
            /*  Player player = new Player
              {
                  Id = playerId,
                  Name = currentPlayer.Name,
                  Money = currentPlayer.Money,
                  Experience = currentPlayer.Experience,
                  LastActionExecutedDateTime = currentPlayer.LastActionExecutedDateTime,
                  CurrentAttackPlayerItemId = currentPlayer.CurrentAttackPlayerItemId,
                  CurrentDefensePlayerItemId = currentPlayer.CurrentDefensePlayerItemId,
                  CurrentFuelPlayerItemId = currentPlayer.CurrentFuelPlayerItemId
              };
  */
            var playerItems = await _playerItemSdk.Find();
            var items = await _itemSdk.Find();


            if (!string.IsNullOrEmpty(serviceResultJson))
            {
                ServiceResult<GameResult> serviceResultObject = JsonSerializer.Deserialize<ServiceResult<GameResult>>(serviceResultJson);

                GameResult gameResult = serviceResultObject.Data;


                GameOverview gameOverview = new GameOverview { GameResult = serviceResultObject.Data, PlayerItems = playerItems, Items = items, ServiceMessages = serviceResultObject.Messages };

                return View(gameOverview);
            }
            else
            {
                GameResult gameResult = new GameResult();
                gameResult.Player = currentPlayer;
                GameOverview gameOverview = new GameOverview { GameResult = gameResult, PlayerItems = playerItems, Items = items };
                return View(gameOverview);
            }
        }

        public async Task<IActionResult> Loot(int playerId)
        {
            ServiceResult<GameResult> serviceResult = await _gameSdk.PerformAction(playerId);

            string serviceResultJson = JsonSerializer.Serialize(serviceResult);
            
            
            return RedirectToAction("Index", new {serviceResultJson = serviceResultJson});
        }
    }
}
