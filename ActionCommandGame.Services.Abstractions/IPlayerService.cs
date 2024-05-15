using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerService
    {
        Task<PlayerResult> Get(int id);
        Task<IList<PlayerResult>> Find();
        Task<PlayerResult> Create(PlayerRequest request);
        Task<PlayerResult> Update(int id, PlayerRequest request);
        Task<bool> Delete(int id);
    }
}
