namespace ActionCommandGame.Security.Model.Abstractions
{
    public interface ITokenStore
    {
        public string? GetToken();
        public void SaveToken(string? bearerToken);
    }
}
