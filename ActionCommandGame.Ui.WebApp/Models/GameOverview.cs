using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Ui.WebApp.Models
{
    public class GameOverview
    {
        public IList<PlayerItemResult> PlayerItems;
        public IList<ItemResult> Items;
        public GameResult GameResult;
        public IList<ServiceMessage> ServiceMessages;
    }
}
