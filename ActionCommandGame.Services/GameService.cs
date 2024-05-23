/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Settings;

namespace ActionCommandGame.Services
{
    public class GameService : IGameService
    {
        private readonly AppSettings _appSettings;
        
        private readonly PlayerSdk _playerSdk;
        private readonly PositiveGameEventSdk _positiveGameEventSdk;
        private readonly NegativeGameEventSdk _negativeGameEventSdk;
        private readonly ItemSdk _itemSdk;
        private readonly PlayerItemSdk _playerItemSdk;

        public GameService(
            AppSettings appSettings,

            PlayerSdk playerSdk,
            PositiveGameEventSdk positiveGameEventSdk,
            NegativeGameEventSdk negativeGameEventSdk,
            ItemSdk itemSdk,
            PlayerItemSdk playerItemSdk)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings)); 
            
            _playerSdk = playerSdk;
            _positiveGameEventSdk = positiveGameEventSdk;
            _negativeGameEventSdk = negativeGameEventSdk;
            _itemSdk = itemSdk;
            _playerItemSdk = playerItemSdk;
        }

        public async Task<ServiceResult<GameResult>> PerformAction(int playerId)
        {
            //Check Cooldown
            var player = await _playerSdk.Get(playerId);
            var elapsedSeconds = DateTime.UtcNow.Subtract(player.LastActionExecutedDateTime).TotalSeconds;
            var cooldownSeconds = _appSettings.DefaultCooldown;
            if (player.CurrentFuelPlayerItemId > 0)
            {
                PlayerItemResult playerItem = await _playerItemSdk.Get(player.CurrentFuelPlayerItemId);
                ItemResult fuelItemResult = await _itemSdk.Get(playerItem.ItemId);
                cooldownSeconds = fuelItemResult.ActionCooldownSeconds;
            }

            if (elapsedSeconds < cooldownSeconds)
            {
                var waitSeconds = Math.Ceiling(cooldownSeconds - elapsedSeconds);
                var waitText = $"You are still a bit tired. You have to wait another {waitSeconds} seconds.";
                return new ServiceResult<GameResult>
                {
                    Data = new GameResult { Player = player },
                    Messages = new List<ServiceMessage> { new ServiceMessage { Code = "Cooldown", Message = waitText } }
                };
            }

            var hasAttackItem = false;
            *//*ItemResult attackItemResult = await ;*//*
            if (player.CurrentAttackPlayerItemId > 0)
            {
                hasAttackItem = true;
            }
            
            
            var positiveGameEvent = await _positiveGameEventSdk.GetRandomPositiveGameEvent(hasAttackItem);
            if (positiveGameEvent == null)
            {
                return new ServiceResult<GameResult>{Messages = 
                    new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "Error",
                            Message = "Something went wrong getting the Positive Game Event.",
                            MessagePriority = MessagePriority.Error
                        }
                    }};
            }

            var negativeGameEvent = await _negativeGameEventSdk.GetRandomNegativeGameEvent();

            var oldLevel = player.GetLevel();

            player.Money += positiveGameEvent.Money;
            player.Experience += positiveGameEvent.Experience;

            var newLevel = player.GetLevel();

            var levelMessages = new List<ServiceMessage>();
            //Check if we leveled up
            if (oldLevel < newLevel)
            {
                levelMessages = new List<ServiceMessage>{new ServiceMessage{Code="LevelUp", Message = $"Congratulations, you arrived at level {newLevel}"}};
            }

            //Consume fuel
            var fuelMessages = await ConsumeFuel(player);

            var attackMessages = new List<ServiceMessage>();
            //Consume attack when we got some loot
            if (positiveGameEvent.Money > 0)
            {
                attackMessages.AddRange(await ConsumeAttack(player));
            }

            var defenseMessages = new List<ServiceMessage>();
            var negativeGameEventMessages = new List<ServiceMessage>();
            if (negativeGameEvent != null)
            {
                //Check defense consumption
                if (player.CurrentDefensePlayerItemId > 0)
                {
                    negativeGameEventMessages.Add(new ServiceMessage { Code = "DefenseWithGear", Message = negativeGameEvent.DefenseWithGearDescription });
                    defenseMessages.AddRange(await ConsumeDefense(player, negativeGameEvent.DefenseLoss));
                }
                else
                {
                    negativeGameEventMessages.Add(new ServiceMessage { Code = "DefenseWithoutGear", Message = negativeGameEvent.DefenseWithoutGearDescription });

                    //If we have no defense item, consume the defense loss from Fuel and Attack
                    defenseMessages.AddRange(await ConsumeFuel(player, negativeGameEvent.DefenseLoss));
                    defenseMessages.AddRange(await ConsumeAttack(player, negativeGameEvent.DefenseLoss));
                }
            }

            var warningMessages = await GetWarningMessages(player);

            player.LastActionExecutedDateTime = DateTime.UtcNow;

            PlayerRequest playerRequest = new PlayerRequest
            {
                Name = player.Name,
                Money = player.Money,
                Experience = player.Experience,
                LastActionExecutedDateTime = player.LastActionExecutedDateTime,
                CurrentAttackPlayerItemId = player.CurrentAttackPlayerItemId,
                CurrentDefensePlayerItemId = player.CurrentDefensePlayerItemId,
                CurrentFuelPlayerItemId = player.CurrentFuelPlayerItemId,
            };

            //Save Player
            await _playerSdk.Update(playerId, playerRequest);

            var gameResult = new GameResult
            {
                Player = player,
                PositiveGameEvent = positiveGameEvent,
                NegativeGameEvent = negativeGameEvent,
                NegativeGameEventMessages = negativeGameEventMessages
            };

            var serviceResult = new ServiceResult<GameResult>
            {
                Data = gameResult
            };

            //Add all the messages to the player
            serviceResult.WithMessages(levelMessages);
            serviceResult.WithMessages(warningMessages);
            serviceResult.WithMessages(fuelMessages);
            serviceResult.WithMessages(attackMessages);
            serviceResult.WithMessages(defenseMessages);

            return serviceResult;
        }

        public async Task<ServiceResult<BuyResult>> Buy(int playerId, int itemId)
        {
            var player = await _playerSdk.Get(playerId);
            if (player == null)
            {
                return new ServiceResult<BuyResult>().PlayerNotFound();
            }

            var item = await _itemSdk.Get(itemId);
            if (item == null)
            {
                return new ServiceResult<BuyResult>().ItemNotFound();
            }

            if (item.Price > player.Money)
            {
                return new ServiceResult<BuyResult>().NotEnoughMoney();
            }

            PlayerItemRequest playerItemRequest = new PlayerItemRequest
            {
                ItemId = itemId,
                PlayerId = playerId,
                RemainingAttack = item.Attack,
                RemainingDefense = item.Defense,
                RemainingFuel = item.Fuel

            };

            var newPlayerItem = await _playerItemSdk.Create(playerItemRequest);
            var playerItemList = await _playerItemSdk.Find(playerId);
            var playerItem = playerItemList.FirstOrDefault(playerItem => playerItem.ItemId == itemId);

            player.Money -= item.Price;

            //SaveChanges
            PlayerRequest playerRequest = new PlayerRequest
            {
                Name = player.Name,
                Money = player.Money,
                Experience = player.Experience,
                LastActionExecutedDateTime = player.LastActionExecutedDateTime,
                CurrentAttackPlayerItemId = player.CurrentAttackPlayerItemId,
                CurrentDefensePlayerItemId = player.CurrentDefensePlayerItemId,
                CurrentFuelPlayerItemId = player.CurrentFuelPlayerItemId,
            };

            await _playerSdk.Update(playerId, playerRequest);

            await activateItem(playerId, playerItem.Id);

            Player buyPlayer = new Player
            {
                Id = player.Id,
                Name = player.Name,
                Money = player.Money,
                Experience = player.Experience,
                LastActionExecutedDateTime = player.LastActionExecutedDateTime,
                CurrentFuelPlayerItemId = player.CurrentFuelPlayerItemId,
                CurrentAttackPlayerItemId = player.CurrentAttackPlayerItemId,
                CurrentDefensePlayerItemId = player.CurrentDefensePlayerItemId
            };
            Item buyItem = new Item
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Fuel = item.Fuel,
                Attack = item.Attack,
                Defense = item.Defense,
                ActionCooldownSeconds = item.ActionCooldownSeconds
            };

            var buyResult = new BuyResult
            {
                Player = buyPlayer,
                Item = buyItem
            };
            return new ServiceResult<BuyResult> { Data = buyResult };
        }

        public async Task activateItem(int playerId, int playerItemId)
        {
            var player = await _playerSdk.Get(playerId);
            var playerItem = await _playerItemSdk.Get(playerItemId);
            var item = await _itemSdk.Get(playerItem.ItemId);

            if (item.Defense > item.Attack && item.Defense > item.Fuel)
            {
                player.CurrentDefensePlayerItemId = playerItemId;
            }
            if (item.Attack > item.Defense && item.Attack > item.Fuel)
            {
                player.CurrentAttackPlayerItemId = playerItemId;
            }
            if (item.Fuel > item.Defense && item.Fuel > item.Attack)
            {
                player.CurrentFuelPlayerItemId = playerItemId;
            }
            PlayerRequest playerRequest = new PlayerRequest
            {
                Name = player.Name,
                Money = player.Money,
                Experience = player.Experience,
                CurrentAttackPlayerItemId = player.CurrentAttackPlayerItemId,
                CurrentDefensePlayerItemId = player.CurrentDefensePlayerItemId,
                CurrentFuelPlayerItemId = player.CurrentFuelPlayerItemId,
                LastActionExecutedDateTime = player.LastActionExecutedDateTime
            };

            await _playerSdk.Update(playerId, playerRequest);
        }



        private async Task<IList<ServiceMessage>> ConsumeFuel(PlayerResult player, int fuelLoss = 1)
        {
            //if verwijderd hasValue
            if (player.CurrentFuelPlayerItemId > 0 )
            {
                PlayerItemResult fuelPlayerItem = await _playerItemSdk.Get(player.CurrentFuelPlayerItemId);
                fuelPlayerItem.RemainingFuel -= fuelLoss;
                if (fuelPlayerItem.RemainingFuel <= 0)
                {
                    await _playerItemSdk.Delete(player.CurrentFuelPlayerItemId);

                    IList<PlayerItemResult> ItemList = await _playerItemSdk.Find();
                    //Load a new Fuel Item from inventory
                    var newFuelPlayerItem = ItemList.Where(pi => pi.PlayerId == player.Id)
                        .Where(pi => pi.RemainingFuel > 0)
                        .OrderByDescending(pi => pi.RemainingFuel).FirstOrDefault();

                    if (newFuelPlayerItem != null)
                    {
                        player.CurrentFuelPlayerItemId = newFuelPlayerItem.Id;
                        ItemResult fuelItem = await _itemSdk.Get(newFuelPlayerItem.ItemId);
                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedFuel",
                            Message = $"Your spaceship was empty and you filled it with a new {fuelItem.Name}. Gas up!"
                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoFood",
                        Message = "The tank of spaceship is empty you only move with the momentum you still haveb",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
                else
                {
                    PlayerItemRequest fuelPlayerItemRequest = new PlayerItemRequest()
                    {
                        PlayerId = fuelPlayerItem.PlayerId,
                        ItemId = fuelPlayerItem.ItemId,
                        RemainingAttack = fuelPlayerItem.RemainingAttack,
                        RemainingDefense = fuelPlayerItem.RemainingDefense,
                        RemainingFuel = fuelPlayerItem.RemainingFuel

                    };
                    await _playerItemSdk.Update(fuelPlayerItem.Id, fuelPlayerItemRequest);
                }

            }

            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> ConsumeAttack(PlayerResult player, int attackLoss = 1)
        {
            //hasValue verwijdert
            if (player.CurrentAttackPlayerItemId >= 0 )
            {
                var oldAttackPlayerItem = await _playerItemSdk.Get(player.CurrentAttackPlayerItemId);
                oldAttackPlayerItem.RemainingAttack -= attackLoss;
                if (oldAttackPlayerItem.RemainingAttack <= 0)
                {
                    await _playerItemSdk.Delete(player.CurrentAttackPlayerItemId);

                    IList<PlayerItemResult> ItemList = await _playerItemSdk.Find();
                    //Load a new Attack Item from inventory
                    var newAttackItem = ItemList.Where(pi => pi.PlayerId == player.Id)
                        .Where(pi => pi.RemainingAttack > 0)
                        .OrderByDescending(pi => pi.RemainingAttack).FirstOrDefault();
                    if (newAttackItem != null)
                    {
                        player.CurrentAttackPlayerItemId = newAttackItem.Id;                        
                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedAttack",
                            Message = $"You just broke {await _itemSdk.Get(oldAttackPlayerItem.ItemId)}. No worries, you swiftly wield a new {await _itemSdk.Get(newAttackItem.ItemId)} Yeah!",

                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoAttack",
                        Message = $"You just broke {await _itemSdk.Get(oldAttackPlayerItem.ItemId)}. This was your last Weapon. Bummer!",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
                PlayerItemRequest attackPlayerItemRequest = new PlayerItemRequest()
                {
                    PlayerId = oldAttackPlayerItem.PlayerId,
                    ItemId = oldAttackPlayerItem.ItemId,
                    RemainingAttack = oldAttackPlayerItem.RemainingAttack,
                    RemainingDefense = oldAttackPlayerItem.RemainingDefense,
                    RemainingFuel = oldAttackPlayerItem.RemainingFuel
                };
                await _playerItemSdk.Update(oldAttackPlayerItem.Id, attackPlayerItemRequest);
            }
            else
            {
                //If we don't have any attack tools, just consume more fuel in stead
                await ConsumeFuel(player);
            }

            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> ConsumeDefense(PlayerResult playerResult, int defenseLoss = 1)
        {
            PlayerItemResult oldDefensePlayerItem = await _playerItemSdk.Get(playerResult.CurrentDefensePlayerItemId);

            // hasvalue is weg
            if (oldDefensePlayerItem != null )
            {
                ItemResult oldDefenseItem = await _itemSdk.Get(oldDefensePlayerItem.ItemId);
                oldDefensePlayerItem.RemainingDefense -= defenseLoss;
                if (oldDefensePlayerItem.RemainingDefense <= 0)
                {
                    await _playerItemSdk.Delete(playerResult.CurrentDefensePlayerItemId);

                    IList<PlayerItemResult> playerItemList = await _playerItemSdk.Find();
                    //Load a new Defense Item from inventory
                    var newDefensePlayerItem = playerItemList.Where(pi => pi.PlayerId == playerResult.Id)
                        .Where(pi => pi.RemainingDefense > 0)
                        .OrderByDescending(pi => pi.RemainingDefense).FirstOrDefault();
                    ;
                    if (newDefensePlayerItem != null)
                    {                        
                        playerResult.CurrentDefensePlayerItemId = newDefensePlayerItem.Id;

                        ItemResult newDefenseItem = await _itemSdk.Get(newDefensePlayerItem.ItemId);

                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedDefense",
                            Message = $"Your {oldDefenseItem.Name} is starting to smell. No worries, you swiftly put on a freshly washed {newDefenseItem.Name}. Yeah!"
                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoAttack",
                        Message = $"You just lost {oldDefenseItem.Name}. You continue without protection. Did I just see something move?",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
                PlayerItemRequest defensePlayerItemRequest = new PlayerItemRequest()
                {
                    PlayerId = oldDefensePlayerItem.PlayerId,
                    ItemId = oldDefensePlayerItem.ItemId,
                    RemainingAttack = oldDefensePlayerItem.RemainingAttack,
                    RemainingDefense = oldDefensePlayerItem.RemainingDefense,
                    RemainingFuel = oldDefensePlayerItem.RemainingFuel
                };
                await _playerItemSdk.Update(oldDefensePlayerItem.Id, defensePlayerItemRequest);
            }
            else
            {
                //If we don't have defensive gear, just consume more fuel in stead.
                await ConsumeFuel(playerResult);
            }

            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> GetWarningMessages(PlayerResult player)
        {
            var serviceMessages = new List<ServiceMessage>();
            PlayerItemResult currentFuelPlayerItem = await _playerItemSdk.Get(player.CurrentFuelPlayerItemId);
            PlayerItemResult currentAttackPlayerItem = await _playerItemSdk.Get(player.CurrentAttackPlayerItemId);
            PlayerItemResult currentDefensePlayerItem = await _playerItemSdk.Get(player.CurrentDefensePlayerItemId);

            if (currentFuelPlayerItem == null)
            {
                var infoText = "Playing without food is hard. You need a long time to recover. Consider buying food from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoFood", Message = infoText, MessagePriority = MessagePriority.Warning });
            }
            if (currentAttackPlayerItem == null)
            {
                var infoText = "Playing without tools is hard. You lost extra fuel. Consider buying tools from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoTools", Message = infoText, MessagePriority = MessagePriority.Warning });
            }
            if (currentDefensePlayerItem == null)
            {
                var infoText = "Playing without gear is hard. You lost extra fuel. Consider buying gear from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoGear", Message = infoText, MessagePriority = MessagePriority.Warning });
            }

            return serviceMessages;
        }
    }
}
*/