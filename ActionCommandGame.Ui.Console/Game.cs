using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Settings;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;

namespace ActionCommandGame.Ui.ConsoleApp
{
    public class Game
    {
        private readonly AppSettings _settings;
        private readonly IGameService _gameService;
        private readonly PlayerSdk _playerSdk;
        private readonly ItemSdk _itemSdk;
        private readonly PlayerItemSdk _playerItemSdk;

        public Game(
            AppSettings settings,
            IGameService gameService,
            PlayerSdk playerSdk,
            ItemSdk itemSdk,
            PlayerItemSdk playerItemSdk)
        {
            _settings = settings;
            _gameService = gameService;
            _playerSdk = playerSdk;
            _itemSdk = itemSdk;
            _playerItemSdk = playerItemSdk;
        }

        //Main Loop
        public async void Start()
        {
            Console.OutputEncoding = Encoding.UTF8;
            ConsoleBlockWriter.Write(_settings.GameName, 4 , ConsoleColor.Blue);
            ConsoleWriter.WriteText($"Play your game. Try typing \"help\" or \"{_settings.ActionCommand}\"", ConsoleColor.Yellow);

            //Get the player from somewhere
            var currentPlayerId = 1006;

            while (true)
            {
                ConsoleWriter.WriteText($"{_settings.CommandPromptText} ", ConsoleColor.DarkGray, false);

                string command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }

                if (CheckCommand(command, new[] { "exit", "quit", "stop" }))
                {
                    break;
                }

                //controleert of het uitvoercommand is
                if (CheckCommand(command, new[] { _settings.ActionCommand }))
                {
                    PerformAction(currentPlayerId);

                    ShowStats(currentPlayerId);
                }

                if (CheckCommand(command, new[] { "shop", "store" }))
                {
                    ShowShop();
                }

                if (CheckCommand(command, new[] { "buy", "purchase", "get" }))
                {
                    var itemId = GetIdParameterFromCommand(command);

                    if (!itemId.HasValue)
                    {
                        ConsoleWriter.WriteText("I have no idea what you mean. I have tagged every item with a number. Please give me that number.", ConsoleColor.Red);
                        continue;
                    }

                    Buy(currentPlayerId, itemId.Value);
                }

                if (CheckCommand(command, new[] { "bal", "balance", "money", "xp", "level", "statistics", "stats", "stat", "info" }))
                {
                    ShowStats(currentPlayerId);
                }

                if (CheckCommand(command, new[] { "leaderboard", "lead", "top", "rank", "ranking" }))
                {
                    var players = await _playerSdk.Find();//
                    players = players.OrderByDescending(p => p.Experience).ToList();
                    await ShowLeaderboard(players, currentPlayerId);
                }

                if (CheckCommand(command, new[] { "inventory", "inv", "bag", "backpack" }))
                {
                    var inventory = await _playerItemSdk.Find(currentPlayerId);
                    await ShowInventory(inventory);
                }

                if (CheckCommand(command, new[] { "?", "help", "h", "commands" }))
                {
                    ShowHelp();
                }
            }

            ConsoleWriter.WriteText("Thank you for playing.", ConsoleColor.Yellow);
            Console.ReadLine();
        }

        private async static Task ShowItem(ItemResult item)
        {
            ConsoleWriter.WriteText($"\t[{item.Id}] {item.Name} €{item.Price}", ConsoleColor.White);
            if (!string.IsNullOrWhiteSpace(item.Description))
            {
                ConsoleWriter.WriteText($"\t\t{item.Description}");
            }
            if (item.Fuel > 0)
            {
                ConsoleWriter.WriteText("\t\tFuel: ", ConsoleColor.White, false);
                ConsoleWriter.WriteText($"{item.Fuel}");
            }
            if (item.Attack > 0)
            {
                ConsoleWriter.WriteText("\t\tAttack: ", ConsoleColor.White, false);
                ConsoleWriter.WriteText($"{item.Attack}");
            }
            if (item.Defense > 0)
            {
                ConsoleWriter.WriteText("\t\tDefense: ", ConsoleColor.White, false);
                ConsoleWriter.WriteText($"{item.Defense}");
            }
            if (item.ActionCooldownSeconds > 0)
            {
                ConsoleWriter.WriteText("\t\tCooldown seconds: ", ConsoleColor.White, false);
                ConsoleWriter.WriteText($"{item.ActionCooldownSeconds}"); 
            }
        }

        private async Task ShowPlayerItem(PlayerItemResult playerItem)
        {
            ItemResult showItem = await _itemSdk.Get(playerItem.ItemId);

            ConsoleWriter.WriteText($"\t{showItem.Name}", ConsoleColor.White);
            if (!string.IsNullOrWhiteSpace(showItem.Description))
            {
                ConsoleWriter.WriteText($"\t\t{showItem.Description}");
            }
            if (showItem.Fuel > 0)
            {
                ConsoleWriter.WriteText($"\t\tFuel: {playerItem.RemainingFuel}/{showItem.Fuel}");
            }
            if (showItem.Attack > 0)
            {
                ConsoleWriter.WriteText($"\t\tAttack: {playerItem.RemainingAttack}/{showItem.Attack}");
            }
            if (showItem.Defense > 0)
            {
                ConsoleWriter.WriteText($"\t\tDefense: {playerItem.RemainingDefense}/{showItem.Defense}");
            }
            if (showItem.ActionCooldownSeconds > 0)
            {
                ConsoleWriter.WriteText($"\t\tCooldown seconds: {showItem.ActionCooldownSeconds}");
            }
        }

        private static bool CheckCommand(string command, IList<string> matchingCommands)
        {
            return matchingCommands.Any(c => command.ToLower().StartsWith(c.ToLower()));
        }

        public async void ShowStats(int playerId)
        {
            var player = await _playerSdk.Get(playerId);
            PlayerItemResult fuelPlayerItem = await _playerItemSdk.Get(player.CurrentFuelPlayerItemId);
                       

            //Check food consumption
            if (fuelPlayerItem != null)
            {
                ItemResult fuelItem = await _itemSdk.Get(fuelPlayerItem.ItemId);
                ConsoleWriter.WriteText($"[{fuelItem.Name}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{fuelPlayerItem.RemainingFuel}/{fuelItem.Fuel}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Fuel] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            PlayerItemResult attackPlayerItem = await _playerItemSdk.Get(player.CurrentAttackPlayerItemId);
                        
            //Check attack consumption
            if (attackPlayerItem != null)
            {
                ItemResult attackItem = await _itemSdk.Get(attackPlayerItem.ItemId);
                ConsoleWriter.WriteText($"[{attackItem.Name}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{attackPlayerItem.RemainingAttack}/{attackItem.Attack}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Attack] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            PlayerItemResult defensePlayerItem = await _playerItemSdk.Get(player.CurrentDefensePlayerItemId);
            //Check defense consumption
            if (defensePlayerItem != null)
            {
                ItemResult defenseItem = await _itemSdk.Get(defensePlayerItem.ItemId);
                ConsoleWriter.WriteText($"[{defenseItem.Name}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{defensePlayerItem.RemainingDefense}/{defenseItem.Defense}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Defense] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            ConsoleWriter.WriteText("[Money] ", ConsoleColor.Yellow, false);
            ConsoleWriter.WriteText($"€{player.Money}  ", null, false);
            ConsoleWriter.WriteText("[Level] ", ConsoleColor.Yellow, false);
            ConsoleWriter.WriteText($"{player.GetLevel()} ({player.Experience}/{player.GetExperienceForNextLevel()})  ", null, false);

            ConsoleWriter.WriteText();
            ConsoleWriter.WriteText();
        }

        private async Task ShowLeaderboard(IList<PlayerResult> players, int currentPlayerId)
        {
            foreach (var player in players)
            {
                var text = $"\tLevel {player.GetLevel()} {player.Name} ({player.Experience})";
                if (player.Id == currentPlayerId)
                {
                    ConsoleWriter.WriteText(text, ConsoleColor.Yellow);
                }
                else
                {
                    ConsoleWriter.WriteText(text);
                }
            }
        }

        private async Task ShowInventory(IList<PlayerItemResult> playerItems)
        {
            foreach (var playerItem in playerItems)
            {
                await ShowPlayerItem(playerItem);
            }
        }

        private void ShowHelp()
        {
            ConsoleWriter.WriteText($"\t{_settings.ActionCommand}: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Do something");

            ConsoleWriter.WriteText($"\tshop: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("See the shop items");

            ConsoleWriter.WriteText($"\tbuy 1: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Buy item number 1 from the shop");

            ConsoleWriter.WriteText($"\tinventory: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Shows your inventory");

            ConsoleWriter.WriteText($"\tstats: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("See your statistics");

            ConsoleWriter.WriteText($"\tleaderboard: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("See the leaderboard");

            ConsoleWriter.WriteText($"\tquit: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Quit the game");

            ConsoleWriter.WriteText($"\thelp: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Well, this one is self explanatory, isn't it? Because you just used it?");

        }

        private async void ShowShop()
        {
            ConsoleWriter.WriteText("Available Shop Items", ConsoleColor.Green);
            var shopItems = await _itemSdk.Find();
            foreach (var item in shopItems)
            {
                await ShowItem(item);
            }
            ConsoleWriter.WriteText();
        }

        private async void PerformAction(int playerId)
        {
            var result = await _gameService.PerformAction(playerId);

            var player = result.Data.Player;
            var positiveGameEvent = result.Data.PositiveGameEvent;
            var negativeGameEvent = result.Data.NegativeGameEvent;

            if (positiveGameEvent != null)
            {
                ConsoleWriter.WriteText($"{string.Format(_settings.ActionText, player.Name)} ",
                    ConsoleColor.Green, false);
                ConsoleWriter.WriteText(positiveGameEvent.Name, ConsoleColor.White);
                if (!string.IsNullOrWhiteSpace(positiveGameEvent.Description))
                {
                    ConsoleWriter.WriteText(positiveGameEvent.Description);
                }
                if (positiveGameEvent.Money > 0)
                {
                    ConsoleWriter.WriteText($"€{positiveGameEvent.Money}", ConsoleColor.Yellow, false);
                    ConsoleWriter.WriteText(" has been added to your account.");
                }
            }

            if (negativeGameEvent != null)
            {
                ConsoleWriter.WriteText(negativeGameEvent.Name, ConsoleColor.Blue);
                if (!string.IsNullOrWhiteSpace(negativeGameEvent.Description))
                {
                    ConsoleWriter.WriteText(negativeGameEvent.Description);
                }
                ConsoleWriter.WriteMessages(result.Data.NegativeGameEventMessages);
            }

            ConsoleWriter.WriteMessages(result.Messages);

            ConsoleWriter.WriteText();
        }

        private async void Buy(int playerId, int itemId)
        {
            var result = await _gameService.Buy(playerId, itemId);

            if (result.IsSuccess)
            {
                ConsoleWriter.WriteText($"You bought {result.Data.Item.Name} for €{result.Data.Item.Price}");
                ConsoleWriter.WriteText($"Thank you for shopping. Your current balance is €{result.Data.Player.Money}.");

                //Check if there are info and warning messages
                var nonErrorMessages =
                    result.Messages.Where(m => m.MessagePriority == MessagePriority.Error).ToList();
                ConsoleWriter.WriteMessages(nonErrorMessages);
            }
            else
            {
                var errorMessages = result.Messages.Where(m => m.MessagePriority == MessagePriority.Error)
                    .ToList();
                ConsoleWriter.WriteMessages(errorMessages);
            }

            Console.WriteLine();
        }

        private int? GetIdParameterFromCommand(string command)
        {
            var commandParts = command.Split(" ");
            if (commandParts.Length == 1)
            {
                return null;
            }
            var idPart = commandParts[1];

            int.TryParse(idPart, out var itemId);
            
            return itemId;
        }


    }
}
