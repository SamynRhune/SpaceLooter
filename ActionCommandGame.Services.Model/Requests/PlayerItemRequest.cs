using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PlayerItemRequest
    {
        [Required]
        public int PlayerId { get; set; }
        [Required]
        public int ItemId { get; set; }
        public int RemainingFuel { get; set; }
        public int RemainingAttack { get; set; }
        public int RemainingDefense { get; set; }

    }
}

