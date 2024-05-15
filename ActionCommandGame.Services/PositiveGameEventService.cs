/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Helpers;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PositiveGameEventService : IPositiveGameEventService
    {
        private readonly ActionButtonGameDbContext _database;

        public PositiveGameEventService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<PositiveGameEventResult> Get(int id)
        {
            return await _database.PositiveGameEvents
               .Select(l => new PositiveGameEventResult
               {
                   Id = l.Id,
                   Name = l.Name,
                   Description = l.Description,
                   Money = l.Money,
                   Experience = l.Experience,
                   Probability = l.Probability

               }).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PositiveGameEventResult> GetRandomPositiveGameEvent(bool hasAttackItem)
        {
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

            var randomPositiveGameEvent = await GameEventHelper.GetRandomPositiveGameEvent(gameEvents);

            PositiveGameEventResult result = new PositiveGameEventResult
            {
                Id = randomPositiveGameEvent.Id,
                Name = randomPositiveGameEvent.Name,
                Description = randomPositiveGameEvent.Description,
                Money = randomPositiveGameEvent.Money,
                Experience = randomPositiveGameEvent.Experience,
                Probability = randomPositiveGameEvent.Probability
            };
            return result;
        }

        public async Task<IList<PositiveGameEventResult>> Find()
        {
            return await _database.PositiveGameEvents
                .Select(l => new PositiveGameEventResult
                {
                    Id = l.Id,
                    Name = l.Name,
                    Description = l.Description,
                    Money = l.Money,
                    Experience = l.Experience,
                    Probability = l.Probability

                }).ToListAsync();
        }

        public async Task<PositiveGameEventResult> Create(PositiveGameEventRequest request)
        {
            var positiveGameEvent = new PositiveGameEvent
            {
                Name = request.Name,
                Description = request.Description,
                Money = request.Money,
                Experience = request.Experience,
                Probability = request.Probability
            };

            _database.PositiveGameEvents.Add(positiveGameEvent);
            await _database.SaveChangesAsync();

            return await Get(positiveGameEvent.Id);
        }

        public async Task<PositiveGameEventResult> Update(int id, PositiveGameEventRequest request)
        {
            var db_positiveGameEvent = await _database.PositiveGameEvents
                .FirstOrDefaultAsync(a => a.Id == id);

            if (request is null)
            {
                return null;
            }

            db_positiveGameEvent.Name = request.Name;
            db_positiveGameEvent.Description = request.Description;
            db_positiveGameEvent.Money = request.Money;
            db_positiveGameEvent.Experience = request.Experience;
            db_positiveGameEvent.Probability = request.Probability;

            await _database.SaveChangesAsync();

            return await Get(id);
        }

        public async Task<bool> Delete(int id)
        {
            var positiveGameEvent = await _database.PositiveGameEvents
               .FirstOrDefaultAsync(a => a.Id == id);

            if (positiveGameEvent is null)
            {
                return false;
            }

            _database.PositiveGameEvents.Remove(positiveGameEvent);

            await _database.SaveChangesAsync();
            return true;
        }
    }
}
*/