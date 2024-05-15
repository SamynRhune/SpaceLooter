using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Security.Model
{
    public class JwtAuthenticationResult: ServiceResult
    {
        public string? Token { get; set; }
    }
}
