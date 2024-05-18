﻿using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerService: IPlayerService
    {
        private readonly ActionButtonGameDbContext _database;

        public PlayerService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<PlayerResult> Get(int id)
        {
            //apart omdat entity er moeite mee heeft
            var player = await _database.Players
         .SingleOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return null;
            }

            var playerResult = new PlayerResult
            {
                Id = player.Id,
                Name = player.Name,
                Money = player.Money,
                Experience = player.Experience,
                LastActionExecutedDateTime = player.LastActionExecutedDateTime,
                CurrentAttackPlayerItemId = player.CurrentAttackPlayerItemId,
                CurrentDefensePlayerItemId = player.CurrentDefensePlayerItemId,
                CurrentFuelPlayerItemId = player.CurrentFuelPlayerItemId,
                IdentityPlayerId = player.IdentityPlayerId
            };

            return playerResult;
        }

        public async Task<PlayerResult> GetIdentityId(string id)
        {
            //apart omdat entity er moeite mee heeft
            var player = await _database.Players
         .SingleOrDefaultAsync(p => p.IdentityPlayerId.Equals(id));

            if (player == null)
            {
                return null;
            }

            var playerResult = new PlayerResult
            {
                Id = player.Id,
                Name = player.Name,
                Money = player.Money,
                Experience = player.Experience,
                LastActionExecutedDateTime = player.LastActionExecutedDateTime,
                CurrentAttackPlayerItemId = player.CurrentAttackPlayerItemId,
                CurrentDefensePlayerItemId = player.CurrentDefensePlayerItemId,
                CurrentFuelPlayerItemId = player.CurrentFuelPlayerItemId,
                IdentityPlayerId = player.IdentityPlayerId
            };

            return playerResult;
        }

        public async Task<IList<PlayerResult>> Find()
        {
            return await _database.Players.Select(l => new PlayerResult 
            {
                Id = l.Id,
                Name = l.Name,
                Money = l.Money,
                Experience = l.Experience,
                LastActionExecutedDateTime = l.LastActionExecutedDateTime,
                CurrentAttackPlayerItemId = l.CurrentAttackPlayerItemId,
                CurrentDefensePlayerItemId = l.CurrentDefensePlayerItemId,
                CurrentFuelPlayerItemId = l.CurrentFuelPlayerItemId,
                IdentityPlayerId = l.IdentityPlayerId

            })
                .ToListAsync();
        }

        public async Task<PlayerResult> Create(PlayerRequest request)
        {
            var player = new Player
            {
                Name = request.Name,
                Money = request.Money,
                Experience = request.Experience,
                LastActionExecutedDateTime = DateTime.Now.AddDays(-1),
                CurrentAttackPlayerItemId = request.CurrentAttackPlayerItemId,
                CurrentDefensePlayerItemId = request.CurrentDefensePlayerItemId,
                CurrentFuelPlayerItemId = request.CurrentFuelPlayerItemId,
                IdentityPlayerId = request.IdentityPlayerId
            };

            _database.Players.Add(player);
            await _database.SaveChangesAsync();

            return await Get(player.Id);
        }

        public async Task<PlayerResult> Update(int id, PlayerRequest request)
        {
            var db_player = await _database.Players.Where(pi => pi.Id == id).FirstOrDefaultAsync();

            if (request is null)
            {
                return null;
            }

            db_player.Name = request.Name;
            db_player.Money = request.Money;
            db_player.Experience = request.Experience;
            db_player.LastActionExecutedDateTime = request.LastActionExecutedDateTime;
            db_player.CurrentAttackPlayerItemId = request.CurrentAttackPlayerItemId;
            db_player.CurrentDefensePlayerItemId = request.CurrentDefensePlayerItemId;
            db_player.CurrentFuelPlayerItemId = request.CurrentFuelPlayerItemId;
            db_player.IdentityPlayerId = request.IdentityPlayerId;

            await _database.SaveChangesAsync();

            return await Get(id);
        }

        public async Task<bool> Delete(int id)
        {
            var player = await _database.Players
               .FirstOrDefaultAsync(a => a.Id == id);

            if (player is null)
            {
                return false;
            }

            _database.Players.Remove(player);

            await _database.SaveChangesAsync();
            return true;
        }

        public async Task<AccountResult> GetAccount(int id)
        {
            //apart omdat entity er moeite mee heeft
            var player = await _database.Players
         .SingleOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return null;
            }
            var account = await _database.AspNetUsers.SingleOrDefaultAsync(a => a.Id.Equals(player.IdentityPlayerId));
            if (account == null)
            {
                return null;
            }

            AccountResult result = new AccountResult
            {
                Email = account.Email,
                UserName = account.UserName,
                PhoneNumber = account.PhoneNumber
            };

            
            return result;
        }

        public async Task<AccountResult> UpdateAccount(int id, AccountRequest request)
        {
            var db_player = await _database.Players.Where(pi => pi.Id == id).FirstOrDefaultAsync();

            if (db_player is null)
            {
                return null;
            }

            var account = await _database.AspNetUsers.SingleOrDefaultAsync(a => a.Id.Equals(db_player.IdentityPlayerId));
            if (account == null)
            {
                return null;
            }

            /*AccountResult result = new AccountResult
            {
                Email = account.Email,
                UserName = account.UserName,
                PhoneNumber = account.PhoneNumber
            };*/

            account.UserName = request.UserName;
            account.Email = request.Email;
            account.PhoneNumber = request.PhoneNumber;
            await _database.SaveChangesAsync();

            db_player.Name = request.UserName;

            await _database.SaveChangesAsync();

            return await GetAccount(id);
        }
    }
}
