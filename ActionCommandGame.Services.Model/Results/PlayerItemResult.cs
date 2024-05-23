namespace ActionCommandGame.Services.Model.Results
{
    public class PlayerItemResult
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public int RemainingFuel { get; set; }
        public int RemainingAttack { get; set; }
        public int RemainingDefense { get; set; }
    }
}
