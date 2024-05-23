using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerItemService
    {
        Task<PlayerItemResult> Get(int id);
        Task<IList<PlayerItemResult>> Find(int? playerId = null);
        Task<ServiceResult<PlayerItemResult>> Create(int playerId, int itemId);
        Task<PlayerItemResult> Update(int id, PlayerItemRequest request);
        Task<ServiceResult> Delete(int id);
    }
}
