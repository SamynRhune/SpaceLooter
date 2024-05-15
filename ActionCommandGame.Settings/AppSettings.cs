namespace ActionCommandGame.Settings
{
    public class AppSettings
    {
        public string CommandPromptText { get; set; }
        public string GameName { get; set; }
        public string ActionCommand { get; set; }
        public string ActionText { get; set; }
        public int DefaultCooldown { get; set; } = 100;
    }
}
