using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Extensions;
using ActionCommandGame.Services.Abstractions;

using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerItemService : IPlayerItemService
    {
        private readonly ActionButtonGameDbContext _database;
        private readonly PlayerService _playerService;

        public PlayerItemService(ActionButtonGameDbContext database,PlayerService playerService)
        {
            _database = database;
            _playerService = playerService;
        }

        public async Task<PlayerItemResult> Get(int id)
        {
            return await _database.PlayerItems
                .Select(l => new PlayerItemResult
                {
                    Id = l.Id,
                    PlayerId = l.PlayerId,
                    ItemId = l.ItemId,
                    RemainingAttack = l.RemainingAttack,
                    RemainingDefense = l.RemainingDefense,
                    RemainingFuel = l.RemainingFuel
                    
                })
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IList<PlayerItemResult>> Find(int? playerId = null)
        {
            var query = _database.PlayerItems.AsQueryable();

            if (playerId.HasValue)
            {
                if(playerId.Value == -1)
                {
                    return null;
                }
                query = query
                    .Where(pi => pi.PlayerId == playerId.Value);

                return await _database.PlayerItems.Select(l => new PlayerItemResult
                {
                    Id = l.Id,
                    PlayerId = l.PlayerId,
                    ItemId = l.ItemId,
                    RemainingAttack = l.RemainingAttack,
                    RemainingDefense = l.RemainingDefense,
                    RemainingFuel = l.RemainingFuel

                }).Where(pi => pi.PlayerId == playerId.Value).ToListAsync();

            }

            return await _database.PlayerItems.Select(l => new PlayerItemResult
            {
                Id = l.Id,
                PlayerId = l.PlayerId,
                ItemId = l.ItemId,
                RemainingAttack = l.RemainingAttack,
                RemainingDefense = l.RemainingDefense,
                RemainingFuel = l.RemainingFuel

            }).ToListAsync();
           
        }

        public async Task<ServiceResult<PlayerItemResult>> Create(int playerId, int itemId)
        {
            var player = _database.Players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                return new ServiceResult<PlayerItemResult>().PlayerNotFound();
            }

            var item = _database.Items.SingleOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return new ServiceResult<PlayerItemResult>().ItemNotFound();
            }

            var playerItem = new PlayerItem
            {
                ItemId = itemId,
                /*Item = item,*/
                PlayerId = playerId,
                RemainingAttack = item.Attack,
                RemainingDefense= item.Defense,
                RemainingFuel= item.Fuel
                /*Player = player*/
            };
            _database.PlayerItems.Add(playerItem);
            /*player.Inventory.Add(playerItem);*/
            /*item.PlayerItems.Add(playerItem);*/

            //Auto Equip the item you bought
            /*if (item.Fuel > 0)
            {
                playerItem.RemainingFuel = item.Fuel;
                player.CurrentFuelPlayerItemId = playerItem.Id;
                *//*player.CurrentFuelPlayerItem = playerItem;*//*
            }
            if (item.Attack > 0)
            {
                playerItem.RemainingAttack = item.Attack;
                player.CurrentAttackPlayerItemId = playerItem.Id;
                *//*player.CurrentAttackPlayerItem = playerItem;*//*
            }
            if (item.Defense > 0)
            {
                playerItem.RemainingDefense = item.Defense;
                player.CurrentDefensePlayerItemId = playerItem.Id;
                *//*player.CurrentDefensePlayerItem = playerItem;*//*
            }*/

            _database.SaveChanges();

            PlayerItemResult result = new PlayerItemResult
                {
                Id = playerItem.Id,
                PlayerId = playerItem.PlayerId,
                ItemId = playerItem.ItemId,
                RemainingAttack = playerItem.RemainingAttack,
                RemainingDefense = playerItem.RemainingDefense,
                RemainingFuel = playerItem.RemainingFuel
                };

            return new ServiceResult<PlayerItemResult>(result);
        }

        public async Task<PlayerItemResult> Update(int id, PlayerItemRequest request)
        {
            var db_playerItem = await _database.PlayerItems
                .FirstOrDefaultAsync(a => a.Id == id);

            if (request is null)
            {
                return null;
            }

            db_playerItem.Id = id;
            db_playerItem.PlayerId = request.PlayerId;
            db_playerItem.ItemId = request.ItemId;
            db_playerItem.RemainingDefense = request.RemainingDefense;
            db_playerItem.RemainingAttack = request.RemainingAttack;
            db_playerItem.RemainingFuel = request.RemainingFuel;
            

            await _database.SaveChangesAsync();

            return await Get(id);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var playerItem = _database.PlayerItems.SingleOrDefault(pi => pi.Id == id);

            if (playerItem == null)
            {
                return new ServiceResult().NotFound();
            }
            
            PlayerResult player = await _playerService.Get(playerItem.PlayerId);
            /*player.Inventory.Remove(playerItem);*/
            
            /*var item = playerItem.Item;
            item.PlayerItems.Remove(playerItem);*/

            //Clear up equipment
            if (player.CurrentFuelPlayerItemId == id)
            {
                player.CurrentFuelPlayerItemId = -1;
                //player.CurrentFuelPlayerItem = null;
            }
            if (player.CurrentAttackPlayerItemId == id)
            {
                player.CurrentAttackPlayerItemId = -1;
                //player.CurrentAttackPlayerItem = null;
            }
            if (player.CurrentDefensePlayerItemId == id)
            {
                player.CurrentDefensePlayerItemId = -1;
                //player.CurrentDefensePlayerItem = null;
            }

            _database.PlayerItems.Remove(playerItem);

            //Save Changes
            _database.SaveChanges();

            return new ServiceResult();
        }
        
    }
}
