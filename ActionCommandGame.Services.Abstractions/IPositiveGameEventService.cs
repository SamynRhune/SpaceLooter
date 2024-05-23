using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPositiveGameEventService
    {
        Task<PositiveGameEventResult> Get(int id);
        Task<PositiveGameEventResult> GetRandomPositiveGameEvent(bool hasAttackItem);
        Task<IList<PositiveGameEventResult>> Find();
        Task<PositiveGameEventResult> Create(PositiveGameEventRequest request);
        Task<PositiveGameEventResult> Update(int id, PositiveGameEventRequest request);
        Task<bool> Delete(int id);
    }
}
