using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
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
                IdentityPlayerId = player.IdentityPlayerId, 
                
            };

            

            return playerResult;
        }

        public async Task<PlayerResult> GetIdentityId(string id)
        {
            
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

        
    }
}
