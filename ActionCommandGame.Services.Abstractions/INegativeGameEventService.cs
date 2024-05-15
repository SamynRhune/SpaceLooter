using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface INegativeGameEventService
    {
        Task<NegativeGameEventResult> Get(int id);
        Task<NegativeGameEventResult> GetRandomNegativeGameEvent();
        Task<IList<NegativeGameEventResult>> Find();
        Task<NegativeGameEventResult> Create(NegativeGameEventRequest request);
        Task<NegativeGameEventResult> Update(int id, NegativeGameEventRequest request);
        Task<bool> Delete(int id);
    }
}
