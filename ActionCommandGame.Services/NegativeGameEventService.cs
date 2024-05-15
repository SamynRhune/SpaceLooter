/*using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class NegativeGameEventService: INegativeGameEventService
    {
        private readonly ActionButtonGameDbContext _database;

        public NegativeGameEventService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<NegativeGameEventResult> Get(int id)
        {
            return await _database.NegativeGameEvents
                .Select(l => new NegativeGameEventResult
                {
                    Id = l.Id,
                    Name = l.Name,
                    Description = l.Description,
                    DefenseWithGearDescription = l.DefenseWithGearDescription,
                    DefenseWithoutGearDescription = l.DefenseWithoutGearDescription,
                    DefenseLoss = l.DefenseLoss,
                    Probability = l.Probability
                    
                })
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<NegativeGameEventResult> GetRandomNegativeGameEvent()
        {
            var gameEvents = await Find();

            var negativeGameEventsList = gameEvents.Select(l => new NegativeGameEvent
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                DefenseWithGearDescription = l.DefenseWithGearDescription,
                DefenseWithoutGearDescription = l.DefenseWithoutGearDescription,
                DefenseLoss = l.DefenseLoss,
                Probability = l.Probability

            }).ToList();
            
            var randomNegativeGameEvent = GameEventHelper.GetRandomNegativeGameEvent(negativeGameEventsList);

            NegativeGameEventResult result =new NegativeGameEventResult
            {
                Id = randomNegativeGameEvent.Id,
                Name = randomNegativeGameEvent?.Name,
                Description = randomNegativeGameEvent?.Description,
                DefenseWithGearDescription = randomNegativeGameEvent?.DefenseWithGearDescription,
                DefenseWithoutGearDescription = randomNegativeGameEvent.DefenseWithoutGearDescription,
                DefenseLoss = randomNegativeGameEvent.DefenseLoss,
                Probability = randomNegativeGameEvent.Probability 
            };
            return result;
            
        }

        public  async Task<IList<NegativeGameEventResult>> Find()
        {
            return await _database.NegativeGameEvents
                .Select(l => new NegativeGameEventResult
                {
                    Id = l.Id,
                    Name = l.Name,
                    Description = l.Description,
                    DefenseWithGearDescription = l.DefenseWithGearDescription,
                    DefenseWithoutGearDescription = l.DefenseWithoutGearDescription,
                    DefenseLoss = l.DefenseLoss,
                    Probability = l.Probability

                }).ToListAsync();
        }

        public async Task<NegativeGameEventResult> Create(NegativeGameEventRequest request)
        {
            var negativeGameEvent = new NegativeGameEvent
            {
                Name = request.Name,
                Description = request.Description,
                DefenseWithGearDescription = request.DefenseWithGearDescription,
                DefenseWithoutGearDescription = request.DefenseWithoutGearDescription,
                DefenseLoss = request.DefenseLoss,
                Probability = request.Probability
            };

            _database.NegativeGameEvents.Add(negativeGameEvent);
            await _database.SaveChangesAsync();

            return await Get(negativeGameEvent.Id);
        }

        public async Task<NegativeGameEventResult> Update(int id, NegativeGameEventRequest request)
        {
            var db_negativeGameEvent = await _database.NegativeGameEvents
                .FirstOrDefaultAsync(a => a.Id == id);

            if (request is null)
            {
                return null;
            }

            db_negativeGameEvent.Name = request.Name;
            db_negativeGameEvent.Description = request.Description;
            db_negativeGameEvent.DefenseWithGearDescription = request.DefenseWithGearDescription;
            db_negativeGameEvent.DefenseWithoutGearDescription = request.DefenseWithoutGearDescription;
            db_negativeGameEvent.DefenseLoss = request.DefenseLoss;
            db_negativeGameEvent.Probability = request.Probability;

            await _database.SaveChangesAsync();

            return await Get(id);
        }

        public async Task<bool> Delete(int id)
        {
            var negativeGameEvent = await _database.NegativeGameEvents
               .FirstOrDefaultAsync(a => a.Id == id);

            if (negativeGameEvent is null)
            {
                return false;
            }

            _database.NegativeGameEvents.Remove(negativeGameEvent);

            await _database.SaveChangesAsync();
            return true;
        }
    }
}
*/