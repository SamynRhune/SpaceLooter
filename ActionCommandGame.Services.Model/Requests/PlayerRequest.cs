using System;
using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PlayerRequest
    {
        [Required]
        public string Name { get; set; }
        public int Money { get; set; } = 100;
        public int Experience { get; set; } = 0;
        public DateTime LastActionExecutedDateTime { get; set; } = DateTime.Now;
        public int CurrentFuelPlayerItemId { get; set; } = -1;
        public int CurrentAttackPlayerItemId { get; set; } = -1;
        public int CurrentDefensePlayerItemId { get; set; } = -1;
        public string IdentityPlayerId {  get; set; }


    }
}

