using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Extensions;
using ActionCommandGame.Services;
using ActionCommandGame.Helpers;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Settings;
using Microsoft.EntityFrameworkCore;


namespace ActionCommandGame.RestApi.Service
{
    public class GameService
    {
        private readonly AppSettings _appSettings;
        private readonly PlayerService _playerService;
        private readonly ActionButtonGameDbContext _database;
        private readonly PlayerItemService _playerItemService;
        private readonly PositiveGameEventService _positiveGameEventService;
        private readonly NegativeGameEventService _negativeGameEventService;

        public GameService(
            AppSettings appSettings,
            PlayerService playerService,
            ActionButtonGameDbContext database,
            PlayerItemService playerItemService,
            PositiveGameEventService positiveGameEventService,
            NegativeGameEventService negativeGameEventService)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _playerService = playerService;
            _database = database;
            _playerItemService = playerItemService;
            _positiveGameEventService = positiveGameEventService;
            _negativeGameEventService = negativeGameEventService;
        }

        public async Task<ServiceResult<GameResult>> PerformAction(int playerId)
        {
            //Check Cooldown
            //SDK moet volledig weg en wordt service
            Player player = await _database.Players.SingleOrDefaultAsync(p => p.Id == playerId);
            if(player == null)
            {
                return new ServiceResult<GameResult>().PlayerNotFound();
            }
            var elapsedSeconds = DateTime.UtcNow.Subtract(player.LastActionExecutedDateTime).TotalSeconds;
            var cooldownSeconds = _appSettings.DefaultCooldown;
            if (player.CurrentFuelPlayerItemId > 0)
            {
                PlayerItem playerItem = await _database.PlayerItems.FirstOrDefaultAsync(a => a.Id == player.CurrentFuelPlayerItemId);
                if (playerItem != null) 
                {
                    Item fuelItemResult = await _database.Items.FirstOrDefaultAsync(a => a.Id == playerItem.ItemId);
                    if (fuelItemResult == null)
                    {
                        return new ServiceResult<GameResult>().ItemNotFound();
                    }
                    cooldownSeconds = fuelItemResult.ActionCooldownSeconds;
                }
                else
                {
                    player.CurrentFuelPlayerItemId = 0;
                }
                
            }

            if (elapsedSeconds < cooldownSeconds)
            {
                var waitSeconds = Math.Ceiling(cooldownSeconds - elapsedSeconds);
                var waitText = $"Your spaceship is too slow. You have to wait another {waitSeconds} seconds.";

                PlayerResult playerServiceResult = await _playerService.Get(playerId);
                return new ServiceResult<GameResult>
                {
                    Data = new GameResult { Player = playerServiceResult },
                    Messages = new List<ServiceMessage> { new ServiceMessage { Code = "Cooldown", Message = waitText } }
                };
            }

            //Het Selecteren van een random Positive game event

            var hasAttackItem = false;
            /*ItemResult attackItemResult = await ;*/
            if (player.CurrentAttackPlayerItemId > 0)
            {
                hasAttackItem = true;
            }

            var query = _database.PositiveGameEvents.AsQueryable();

            //If we don't have an attack item, we can only get low-reward items.
            if (!hasAttackItem)
            {
                query = query.Where(p => p.Money < 50);
            }

            var gameEvents = query.Select(l => new PositiveGameEvent
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                Money = l.Money,
                Experience = l.Experience,
                Probability = l.Probability

            }).ToList();

            PositiveGameEvent randomPositiveGameEvent = await GameEventHelper.GetRandomPositiveGameEvent(gameEvents);

            

            //BEEINDIGEN van positive gameEvent

            
            if (randomPositiveGameEvent == null)
            {
                return new ServiceResult<GameResult>
                {
                    Messages =
                    new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "Error",
                            Message = "Something went wrong getting the Positive Game Event.",
                            MessagePriority = MessagePriority.Error
                        }
                    }
                };
            }

            //Begin negative game event
            var negativeGameEvents =  await _database.NegativeGameEvents
                .ToListAsync();

            var negativeGameEventsList = negativeGameEvents.Select(l => new NegativeGameEvent
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                DefenseWithGearDescription = l.DefenseWithGearDescription,
                DefenseWithoutGearDescription = l.DefenseWithoutGearDescription,
                DefenseLoss = l.DefenseLoss,
                Probability = l.Probability

            }).ToList();

            NegativeGameEvent randomNegativeGameEvent = GameEventHelper.GetRandomNegativeGameEvent(negativeGameEventsList);
            //Einde NEGATIVE GAME EVENT
            //var negativeGameEvent = await _negativeGameEventSdk.GetRandomNegativeGameEvent();



            PlayerResult playerResult = await _playerService.Get(playerId);
            var oldLevel = playerResult.GetLevel();

            player.Money += randomPositiveGameEvent.Money;
            player.Experience += randomPositiveGameEvent.Experience;
            playerResult.Experience += randomPositiveGameEvent.Experience;

            var newLevel = playerResult.GetLevel();

            var levelMessages = new List<ServiceMessage>();
            //Check if we leveled up
            if (oldLevel < newLevel)
            {
                levelMessages = new List<ServiceMessage> { new ServiceMessage { Code = "LevelUp", Message = $"Congratulations, you arrived at level {newLevel}" } };
            }

            //Consume fuel
            var fuelMessages = await ConsumeFuel(player);

            var attackMessages = new List<ServiceMessage>();
            //Consume attack when we got some loot
            if (randomPositiveGameEvent.Money > 0)
            {
                attackMessages.AddRange(await ConsumeAttack(player));
            }

            var defenseMessages = new List<ServiceMessage>();
            var negativeGameEventMessages = new List<ServiceMessage>();
            if (randomNegativeGameEvent != null)
            {
                //Check defense consumption
                if (player.CurrentDefensePlayerItemId > 0)
                {
                    negativeGameEventMessages.Add(new ServiceMessage { Code = "DefenseWithGear", Message = randomNegativeGameEvent.DefenseWithGearDescription });
                    defenseMessages.AddRange(await ConsumeDefense(player, randomNegativeGameEvent.DefenseLoss));
                }
                else
                {
                    negativeGameEventMessages.Add(new ServiceMessage { Code = "DefenseWithoutGear", Message = randomNegativeGameEvent.DefenseWithoutGearDescription });

                    //If we have no defense item, consume the defense loss from Fuel and Attack
                    defenseMessages.AddRange(await ConsumeFuel(player, randomNegativeGameEvent.DefenseLoss));
                    defenseMessages.AddRange(await ConsumeAttack(player, randomNegativeGameEvent.DefenseLoss));
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
                IdentityPlayerId = player.IdentityPlayerId,
            };

            //Save Player
            await _playerService.Update(playerId, playerRequest);

            playerResult = await _playerService.Get(playerId);
            PositiveGameEventResult positiveGameEventResult = await _positiveGameEventService.Get(randomPositiveGameEvent.Id);
            NegativeGameEventResult negativeGameEventResult = null;
            if (randomNegativeGameEvent != null)
            {
                negativeGameEventResult = await _negativeGameEventService.Get(randomNegativeGameEvent.Id);
            }
            

            var gameResult = new GameResult
            {
                Player = playerResult,
                PositiveGameEvent = positiveGameEventResult,
                NegativeGameEvent = negativeGameEventResult,
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
            Player player = await _database.Players.SingleOrDefaultAsync(p => p.Id == playerId);
            if (player == null)
            {
                return new ServiceResult<BuyResult>().PlayerNotFound();
            }

            Item item = await _database.Items.FirstOrDefaultAsync(a => a.Id == itemId);
            if (item == null)
            {
                return new ServiceResult<BuyResult>().ItemNotFound();
            }

            if (item.Price > player.Money)
            {
                return new ServiceResult<BuyResult>().NotEnoughMoney();
            }

            var newPlayerItem = await _playerItemService.Create(playerId,itemId);
            var playerItemList = await _database.PlayerItems.Select(l => new PlayerItemResult
            {
                Id = l.Id,
                PlayerId = l.PlayerId,
                ItemId = l.ItemId,
                RemainingAttack = l.RemainingAttack,
                RemainingDefense = l.RemainingDefense,
                RemainingFuel = l.RemainingFuel

            }).Where(pi => pi.PlayerId == playerId).ToListAsync();

            // controleer voor persoon die geen items heeft
            if(playerItemList == null)
            {
                return new ServiceResult<BuyResult>().NotFound();
            }

            var playerItem = playerItemList.FirstOrDefault(playerItem => playerItem.ItemId == itemId);
            if (playerItem == null)
            {
                return new ServiceResult<BuyResult>().ItemNotFound();
            }

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
                IdentityPlayerId = player.IdentityPlayerId,
            };
            // update player
            await _playerService.Update(playerId, playerRequest);

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
                CurrentDefensePlayerItemId = player.CurrentDefensePlayerItemId,
                IdentityPlayerId = player.IdentityPlayerId,
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
            Player player = await _database.Players.SingleOrDefaultAsync(p => p.Id == playerId);
            PlayerItem playerItem = await _database.PlayerItems.FirstOrDefaultAsync(a => a.Id == playerItemId);
            Item item = await _database.Items.FirstOrDefaultAsync(a => a.Id == playerItem.ItemId);

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
                LastActionExecutedDateTime = player.LastActionExecutedDateTime,
                IdentityPlayerId = player.IdentityPlayerId,
            };

            await _playerService.Update(playerId, playerRequest);
        }



        private async Task<IList<ServiceMessage>> ConsumeFuel(Player player, int fuelLoss = 1)
        {
            //if verwijderd hasValue
            if (player.CurrentFuelPlayerItemId > 0)
            {
                PlayerItem fuelPlayerItem = await _database.PlayerItems.FirstOrDefaultAsync(a => a.Id == player.CurrentFuelPlayerItemId);
                fuelPlayerItem.RemainingFuel -= fuelLoss;
                if (fuelPlayerItem.RemainingFuel <= 0)
                {
                    await _playerItemService.Delete(player.CurrentFuelPlayerItemId);

                    IList<PlayerItem> ItemList = await _database.PlayerItems.ToListAsync();
                    //Load a new Fuel Item from inventory
                    var newFuelPlayerItem = ItemList.Where(pi => pi.PlayerId == player.Id)
                        .Where(pi => pi.RemainingFuel > 0)
                        .OrderByDescending(pi => pi.RemainingFuel).FirstOrDefault();

                    if (newFuelPlayerItem != null)
                    {
                        player.CurrentFuelPlayerItemId = newFuelPlayerItem.Id;
                        Item fuelItem = await _database.Items.FirstOrDefaultAsync(a => a.Id == newFuelPlayerItem.ItemId);
                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedFuel",
                            Message = $"Your spaceship was empty and you filled it with a new {fuelItem.Name}. Gas up!"
                        }};
                    }
                    else
                    {
                        player.CurrentFuelPlayerItemId = 0;
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoFood",
                        Message = "The tank of spaceship is empty you only move with the momentum you still have.",
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
                    await _playerItemService.Update(fuelPlayerItem.Id, fuelPlayerItemRequest);
                }

            }

            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> ConsumeAttack(Player player, int attackLoss = 1)
        {
            //hasValue verwijdert
            if (player.CurrentAttackPlayerItemId >= 0)
            {
                var oldAttackPlayerItem = await _database.PlayerItems.FirstOrDefaultAsync(a => a.Id == player.CurrentAttackPlayerItemId);
                oldAttackPlayerItem.RemainingAttack -= attackLoss;
                if (oldAttackPlayerItem.RemainingAttack <= 0)
                {
                    await _playerItemService.Delete(player.CurrentAttackPlayerItemId);

                    IList<PlayerItem> ItemList = await _database.PlayerItems.ToListAsync();
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
                            Message = $"You just broke {await _database.Items.FirstOrDefaultAsync(a => a.Id == oldAttackPlayerItem.ItemId)}. No worries, you swiftly wield a new {await _database.Items.FirstOrDefaultAsync(a => a.Id == newAttackItem.ItemId)} Yeah!",

                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoAttack",
                        Message = $"You just broke {await _database.Items.FirstOrDefaultAsync(a => a.Id == oldAttackPlayerItem.ItemId)}. This was your last Weapon. Bummer!",
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
                await _playerItemService.Update(oldAttackPlayerItem.Id, attackPlayerItemRequest);
            }
            else
            {
                //If we don't have any attack tools, just consume more fuel in stead
                await ConsumeFuel(player);
            }

            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> ConsumeDefense(Player playerResult, int defenseLoss = 1)
        {
            PlayerItem oldDefensePlayerItem = await _database.PlayerItems.FirstOrDefaultAsync(a => a.Id == playerResult.CurrentDefensePlayerItemId);

            // hasvalue is weg
            if (oldDefensePlayerItem != null)
            {
                Item oldDefenseItem = await _database.Items.FirstOrDefaultAsync(a => a.Id == oldDefensePlayerItem.ItemId);
                oldDefensePlayerItem.RemainingDefense -= defenseLoss;
                if (oldDefensePlayerItem.RemainingDefense <= 0)
                {
                    await _playerItemService.Delete(playerResult.CurrentDefensePlayerItemId);

                    IList<PlayerItem> playerItemList = await _database.PlayerItems.ToListAsync();
                    //Load a new Defense Item from inventory
                    var newDefensePlayerItem = playerItemList.Where(pi => pi.PlayerId == playerResult.Id)
                        .Where(pi => pi.RemainingDefense > 0)
                        .OrderByDescending(pi => pi.RemainingDefense).FirstOrDefault();
                    ;
                    if (newDefensePlayerItem != null)
                    {
                        playerResult.CurrentDefensePlayerItemId = newDefensePlayerItem.Id;

                        Item newDefenseItem = await _database.Items.FirstOrDefaultAsync(a => a.Id == newDefensePlayerItem.ItemId);

                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedDefense",
                            Message = $"Your {oldDefenseItem.Name} is breaking down. No worries, you swiftly activated a new {newDefenseItem.Name}. Yeah!"
                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoAttack",
                        Message = $"You just lost {oldDefenseItem.Name}. You continue without shield. Did I just see something move?",
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
                await _playerItemService.Update(oldDefensePlayerItem.Id, defensePlayerItemRequest);
            }
            else
            {
                //If we don't have defensive gear, just consume more fuel in stead.
                await ConsumeFuel(playerResult);
            }

            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> GetWarningMessages(Player player)
        {
            var serviceMessages = new List<ServiceMessage>();
            PlayerItem currentFuelPlayerItem = await _database.PlayerItems.FirstOrDefaultAsync(a => a.Id == player.CurrentFuelPlayerItemId);
            PlayerItem currentAttackPlayerItem = await _database.PlayerItems.FirstOrDefaultAsync(a => a.Id == player.CurrentAttackPlayerItemId);
            PlayerItem currentDefensePlayerItem = await _database.PlayerItems.FirstOrDefaultAsync(a => a.Id == player.CurrentDefensePlayerItemId);

            if (currentFuelPlayerItem == null)
            {
                var infoText = "Playing without fuel is hard. You need a long time to have the right speed. Consider buying fuel from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoFood", Message = infoText, MessagePriority = MessagePriority.Warning });
            }
            if (currentAttackPlayerItem == null)
            {
                var infoText = "Playing without weapons is hard. You lost extra fuel. Consider buying weapons from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoTools", Message = infoText, MessagePriority = MessagePriority.Warning });
            }
            if (currentDefensePlayerItem == null)
            {
                var infoText = "Playing without shield is hard. You lost extra fuel. Consider buying a shield from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoGear", Message = infoText, MessagePriority = MessagePriority.Warning });
            }

            return serviceMessages;
        }
    }
}
