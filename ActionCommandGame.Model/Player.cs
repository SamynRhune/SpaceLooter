using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ActionCommandGame.Model.Abstractions;

namespace ActionCommandGame.Model
{
    public class Player: IIdentifiable
    {
        /*public Player()
        {
            Inventory = new List<PlayerItem>();
        }*/

        public int Id { get; set; }
        public string Name { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public DateTime LastActionExecutedDateTime { get; set; } = DateTime.Now;

        public int CurrentFuelPlayerItemId { get; set; }
      /*  [JsonIgnore]
        public PlayerItem CurrentFuelPlayerItem { get; set; }*/
        public int CurrentAttackPlayerItemId { get; set; }
        /*[JsonIgnore]
        public PlayerItem CurrentAttackPlayerItem { get; set; }*/
        public int CurrentDefensePlayerItemId { get; set; }
        public string IdentityPlayerId { get; set; }
        /*[JsonIgnore]
        public PlayerItem CurrentDefensePlayerItem { get; set; }
        [JsonIgnore]
        public IList<PlayerItem> Inventory { get; set; }*/

    }
}
