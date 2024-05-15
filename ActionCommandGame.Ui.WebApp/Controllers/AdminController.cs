using ActionCommandGame.Model;
using ActionCommandGame.Sdk;
using ActionCommandGame.Security.Model;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActionCommandGame.Ui.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ItemSdk _itemSdk;
        private readonly PlayerItemSdk _playerItemSdk;
        private readonly PlayerSdk _playerSdk;
        private readonly PositiveGameEventSdk _positiveGameEventSdk;
        private readonly NegativeGameEventSdk _negativeGameEventSdk;


        
        public AdminController(ItemSdk itemSdk, PlayerItemSdk playerItemSdk, PlayerSdk playerSdk, PositiveGameEventSdk positiveGameEventSdk, NegativeGameEventSdk negativeGameEventSdk)
        {
            _itemSdk = itemSdk;
            _playerItemSdk = playerItemSdk;
            _playerSdk = playerSdk;
            _positiveGameEventSdk = positiveGameEventSdk;
            _negativeGameEventSdk = negativeGameEventSdk;
        }

        
        public IActionResult Index()
        {            
            return View();
        }

        public async Task<IActionResult> ItemIndex()
        {
            var itemList = await _itemSdk.Find();
            return View("Items/Index", itemList);
        }

        [HttpGet]
        public async Task<IActionResult> ItemCreate()
        {
            return View("Items/Create");
        }

        [HttpPost]
        public async Task<IActionResult> ItemCreate(Item newItem)
        {
            ItemRequest itemRequest = new ItemRequest
            {
                Name = newItem.Name,
                Description = newItem.Description,
                Price = newItem.Price,
                Fuel = newItem.Fuel,
                Attack = newItem.Attack,
                Defense = newItem.Defense,
                ActionCooldownSeconds = newItem.ActionCooldownSeconds,
            };
            await _itemSdk.Create(itemRequest);
            
            return RedirectToAction("ItemIndex");
        }

        public async Task<IActionResult> ItemDelete(int id)
        {
            await _itemSdk.Delete(id);
            return RedirectToAction("ItemIndex");
        }

        [HttpGet]
        public async Task<IActionResult> ItemEdit(int id)
        {
            var item = await _itemSdk.Get(id);

            Item editItem = new Item
            {
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Fuel = item.Fuel,
                Attack = item.Attack,
                Defense = item.Defense,
                ActionCooldownSeconds = item.ActionCooldownSeconds,
            };

            return View("Items/Edit", editItem);
        }

        [HttpPost]
        public async Task<IActionResult> ItemEdit(int id, Item newItem)
        {
            ItemRequest itemRequest = new ItemRequest
            {
                Name = newItem.Name,
                Description = newItem.Description,
                Price = newItem.Price,
                Fuel = newItem.Fuel,
                Attack = newItem.Attack,
                Defense = newItem.Defense,
                ActionCooldownSeconds = newItem.ActionCooldownSeconds,
            };
            await _itemSdk.Update(id, itemRequest);
            return RedirectToAction("ItemIndex");
        }

        public async Task<IActionResult> PlayerItemIndex()
        {
            var playerItemList = await _playerItemSdk.Find();
            return View("PlayerItems/Index", playerItemList);
        }

        [HttpGet]
        public async Task<IActionResult> PlayerItemCreate()
        {
            return View("PlayerItems/Create");
        }

        [HttpPost]
        public async Task<IActionResult> PlayerItemCreate(PlayerItem newItem)
        {
            PlayerItemRequest playerItemRequest = new PlayerItemRequest
            {
                PlayerId = newItem.PlayerId,
                ItemId = newItem.ItemId,
                RemainingAttack = newItem.RemainingAttack,
                RemainingDefense = newItem.RemainingDefense,
                RemainingFuel = newItem.RemainingFuel
            };
            await _playerItemSdk.Create(playerItemRequest);

            return RedirectToAction("PlayerItemIndex");
        }

        public async Task<IActionResult> PlayerItemDelete(int id)
        {
            await _playerItemSdk.Delete(id);
            return RedirectToAction("PlayerItemIndex");
        }

        [HttpGet]
        public async Task<IActionResult> PlayerItemEdit(int id)
        {
            var playerItem = await _playerItemSdk.Get(id);

            PlayerItem editItem = new PlayerItem
            {
                PlayerId = playerItem.PlayerId,
                ItemId = playerItem.ItemId,
                RemainingAttack = playerItem.RemainingAttack,
                RemainingDefense = playerItem.RemainingDefense,
                RemainingFuel = playerItem.RemainingFuel
            };

            return View("PlayerItems/Edit", editItem);
        }

        [HttpPost]
        public async Task<IActionResult> PlayerItemEdit(int id, PlayerItem newPlayerItem)
        {
            PlayerItemRequest editItem = new PlayerItemRequest
            {
                PlayerId = newPlayerItem.PlayerId,
                ItemId = newPlayerItem.ItemId,
                RemainingAttack = newPlayerItem.RemainingAttack,
                RemainingDefense = newPlayerItem.RemainingDefense,
                RemainingFuel = newPlayerItem.RemainingFuel
            };
            await _playerItemSdk.Update(id, editItem);
            return RedirectToAction("PlayerItemIndex");
        }

        public async Task<IActionResult> PlayerIndex()
        {
            var playerList = await _playerSdk.Find();
            return View("Players/Index", playerList);
        }

        [HttpGet]
        public async Task<IActionResult> PlayerCreate()
        {
            return View("Players/Create");
        }

        [HttpPost]
        public async Task<IActionResult> PlayerCreate(Player newPlayer)
        {
            PlayerRequest playerRequest = new PlayerRequest
            {
                Name = newPlayer.Name,
                Money = newPlayer.Money,
                Experience = newPlayer.Experience,
                LastActionExecutedDateTime = newPlayer.LastActionExecutedDateTime,
                CurrentAttackPlayerItemId = newPlayer.CurrentDefensePlayerItemId,
                CurrentDefensePlayerItemId = newPlayer.CurrentFuelPlayerItemId,
                CurrentFuelPlayerItemId = newPlayer.CurrentFuelPlayerItemId
            };
            await _playerSdk.Create(playerRequest);

            return RedirectToAction("PlayerIndex");
        }

        public async Task<IActionResult> PlayerDelete(int id)
        {
            await _playerSdk.Delete(id);
            return RedirectToAction("PlayerIndex");
        }

        [HttpGet]
        public async Task<IActionResult> PlayerEdit(int id)
        {
            var player = await _playerSdk.Get(id);

            Player editPlayer = new Player
            {
                Name = player.Name,
                Money = player.Money,
                Experience = player.Experience,
                LastActionExecutedDateTime = player.LastActionExecutedDateTime,
                CurrentAttackPlayerItemId = player.CurrentDefensePlayerItemId,
                CurrentDefensePlayerItemId = player.CurrentFuelPlayerItemId,
                CurrentFuelPlayerItemId = player.CurrentFuelPlayerItemId
            };

            return View("Players/Edit", editPlayer);
        }

        [HttpPost]
        public async Task<IActionResult> PlayerEdit(int id, Player newPlayer)
        {
            PlayerRequest editPlayer = new PlayerRequest
            {
                Name = newPlayer.Name,
                Money = newPlayer.Money,
                Experience = newPlayer.Experience,
                LastActionExecutedDateTime = newPlayer.LastActionExecutedDateTime,
                CurrentAttackPlayerItemId = newPlayer.CurrentDefensePlayerItemId,
                CurrentDefensePlayerItemId = newPlayer.CurrentFuelPlayerItemId,
                CurrentFuelPlayerItemId = newPlayer.CurrentFuelPlayerItemId
            };
            await _playerSdk.Update(id, editPlayer);
            return RedirectToAction("PlayerIndex");
        }

        public async Task<IActionResult> PositiveEventIndex()
        {
            var positiveEventList = await _positiveGameEventSdk.Find();
            return View("PositiveEvents/Index", positiveEventList);
        }

        [HttpGet]
        public async Task<IActionResult> PositiveEventCreate()
        {
            return View("PositiveEvents/Create");
        }

        [HttpPost]
        public async Task<IActionResult> PositiveEventCreate(PositiveGameEvent newPositiveEvent)
        {
            PositiveGameEventRequest positiveEventRequest = new PositiveGameEventRequest
            {
                Name = newPositiveEvent.Name,
                Money = newPositiveEvent.Money,
                Experience = newPositiveEvent.Experience,
                Description = newPositiveEvent.Description,
                Probability = newPositiveEvent.Probability
            };
            await _positiveGameEventSdk.Create(positiveEventRequest);

            return RedirectToAction("PositiveEventIndex");
        }

        public async Task<IActionResult> PositiveEventDelete(int id)
        {
            await _positiveGameEventSdk.Delete(id);
            return RedirectToAction("PositiveEventIndex");
        }

        [HttpGet]
        public async Task<IActionResult> PositiveEventEdit(int id)
        {
            var positiveEvent = await _positiveGameEventSdk.Get(id);

            PositiveGameEvent editPositiveEvent = new PositiveGameEvent
            {
                Name = positiveEvent.Name,
                Money = positiveEvent.Money,
                Experience = positiveEvent.Experience,
                Description = positiveEvent.Description,
                Probability = positiveEvent.Probability
            };

            return View("PositiveEvents/Edit", editPositiveEvent);
        }

        [HttpPost]
        public async Task<IActionResult> PositiveEventEdit(int id, PositiveGameEvent newPositiveEvent)
        {
            PositiveGameEventRequest editPositiveEvent = new PositiveGameEventRequest
            {
                Name = newPositiveEvent.Name,
                Money = newPositiveEvent.Money,
                Experience = newPositiveEvent.Experience,
                Description = newPositiveEvent.Description,
                Probability = newPositiveEvent.Probability
            };
            await _positiveGameEventSdk.Update(id, editPositiveEvent);
            return RedirectToAction("PositiveEventIndex");
        }

        public async Task<IActionResult> NegativeEventIndex()
        {
            var negativeEventList = await _negativeGameEventSdk.Find();
            return View("NegativeEvents/Index", negativeEventList);
        }

        [HttpGet]
        public async Task<IActionResult> NegativeEventCreate()
        {
            return View("NegativeEvents/Create");
        }

        [HttpPost]
        public async Task<IActionResult> NegativeEventCreate(NegativeGameEvent newNegativeEvent)
        {
            NegativeGameEventRequest negativeEventRequest = new NegativeGameEventRequest
            {
                Name = newNegativeEvent.Name,
                DefenseLoss = newNegativeEvent.DefenseLoss,
                DefenseWithGearDescription = newNegativeEvent.DefenseWithGearDescription,
                DefenseWithoutGearDescription = newNegativeEvent.DefenseWithoutGearDescription,
                Description = newNegativeEvent.Description,
                Probability = newNegativeEvent.Probability
            };
            await _negativeGameEventSdk.Create(negativeEventRequest);

            return RedirectToAction("NegativeEventIndex");
        }

        public async Task<IActionResult> NegativeEventDelete(int id)
        {
            await _negativeGameEventSdk.Delete(id);
            return RedirectToAction("NegativeEventIndex");
        }

        [HttpGet]
        public async Task<IActionResult> NegativeEventEdit(int id)
        {
            var negativeEvent = await _negativeGameEventSdk.Get(id);

            NegativeGameEvent editNegativeEvent = new NegativeGameEvent
            {
                Name = negativeEvent.Name,
                DefenseLoss = negativeEvent.DefenseLoss,
                DefenseWithGearDescription = negativeEvent.DefenseWithGearDescription,
                DefenseWithoutGearDescription = negativeEvent.DefenseWithoutGearDescription,
                Description = negativeEvent.Description,
                Probability = negativeEvent.Probability
            };

            return View("NegativeEvents/Edit", editNegativeEvent);
        }

        [HttpPost]
        public async Task<IActionResult> NegativeEventEdit(int id, NegativeGameEvent newNegativeEvent)
        {
            NegativeGameEventRequest editNegativeEvent = new NegativeGameEventRequest
            {
                Name = newNegativeEvent.Name,
                DefenseLoss = newNegativeEvent.DefenseLoss,
                DefenseWithGearDescription = newNegativeEvent.DefenseWithGearDescription,
                DefenseWithoutGearDescription = newNegativeEvent.DefenseWithoutGearDescription,
                Description = newNegativeEvent.Description,
                Probability = newNegativeEvent.Probability
            };
            await _negativeGameEventSdk.Update(id, editNegativeEvent);
            return RedirectToAction("NegativeEventIndex");
        }
    }
}
