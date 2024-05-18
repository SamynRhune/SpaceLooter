using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActionCommandGame.RestApi.Security.Helpers;
using ActionCommandGame.RestApi.Security.Settings;
using ActionCommandGame.Security.Model;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ActionCommandGame.RestApi.Security
{
    public class IdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly PlayerService _playerService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, PlayerService playerService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _playerService = playerService;
            _roleManager = roleManager;
        }

        public async Task<JwtAuthenticationResult> SignIn(UserSignInRequest request)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret) || !_jwtSettings.Expiry.HasValue)
            {
                return JwtAuthenticationHelper.JwtConfigurationError();
            }

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return JwtAuthenticationHelper.InvalidLogin();
            }

            var roles = await _userManager.GetRolesAsync(user);
            
            //hier komt foutmelding op
            //var roles2 = await _roleManager.GetClaimsAsync(new IdentityRole(RoleConstants.Admin));

            var token = GenerateJwtToken(user, roles, _jwtSettings.Secret, _jwtSettings.Expiry.Value);

            return new JwtAuthenticationResult
            {
                Token = token
            };
        }

        public async Task<JwtAuthenticationResult> Register(UserRegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret) || !_jwtSettings.Expiry.HasValue)
            {
                return JwtAuthenticationHelper.JwtConfigurationError();
            }

            if (_userManager.Users.Any(u => u.UserName == request.UserName))
            {
                return JwtAuthenticationHelper.UserExists();
            }

            var identityUser = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.UserName
            };

            var result = await _userManager.CreateAsync(identityUser, request.Password);

            if (!result.Succeeded)
            {
                return JwtAuthenticationHelper.RegisterError(result.Errors);
            }

            if (!await _roleManager.RoleExistsAsync(RoleConstants.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(RoleConstants.User));
            }
            if(!await _userManager.IsInRoleAsync(identityUser, RoleConstants.User)){
                var roleResult = await _userManager.AddToRoleAsync(identityUser, RoleConstants.User);
                if (!roleResult.Succeeded)
                {
                    return JwtAuthenticationHelper.JwtConfigurationError();
                }
            }
            else
            {
                return JwtAuthenticationHelper.JwtConfigurationError();
            }
            

            

            var playerRequest = new PlayerRequest
            {
                IdentityPlayerId = identityUser.Id,
                Name = request.UserName,
            };

            await _playerService.Create(playerRequest);

            var roles = new List<string> { RoleConstants.Admin };
            var token = GenerateJwtToken(identityUser, roles, _jwtSettings.Secret, _jwtSettings.Expiry.Value);

            return new JwtAuthenticationResult
            {
                Token = token
            };
        }

        public async Task<IdentityUser> GetIdentityUserFromName(string userName)
        {
            var user = await _userManager.FindByIdAsync(userName);
            return user;
        }

        public async Task<JwtAuthenticationResult> SetUserAsUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret) || !_jwtSettings.Expiry.HasValue)
            {
                return JwtAuthenticationHelper.JwtConfigurationError();
            }
            var user = await _userManager.FindByIdAsync(userName);
            if (user == null)
            {
                return JwtAuthenticationHelper.JwtConfigurationError();
            }
            var roles = new List<string> { RoleConstants.User };
            var token = GenerateJwtToken(user, roles, _jwtSettings.Secret, _jwtSettings.Expiry.Value);

            return new JwtAuthenticationResult
            {
                Token = token
            };
        }

        public string GenerateJwtToken(IdentityUser user, IEnumerable<string> roles, string secret, TimeSpan expiry)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(expiry),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
