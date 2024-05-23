using Microsoft.AspNetCore.Identity;
using ActionCommandGame.Security.Model;
using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.RestApi.Security.Helpers
{
    public static class JwtAuthenticationHelper
    {
        public static JwtAuthenticationResult InvalidLogin()
        {
            return new JwtAuthenticationResult
            {
                Messages = new List<ServiceMessage>()
                {
                    new ServiceMessage
                    {
                        Code = "InvalidLogin",
                        Message = "Invalid login attempt",
                        MessagePriority = MessagePriority.Error
                    }
                }
            };
        }

        public static JwtAuthenticationResult UserExists()
        {
            return new JwtAuthenticationResult
            {
                Messages = new List<ServiceMessage>()
                {
                    new ServiceMessage
                    {
                        Code = "UserExists",
                        Message = "User already exists",
                        MessagePriority = MessagePriority.Error
                    }
                }
            };
        }

        public static JwtAuthenticationResult JwtConfigurationError()
        {
            return new JwtAuthenticationResult
            {
                Messages = new List<ServiceMessage>()
                {
                    new ServiceMessage
                    {
                        Code = "JwtConfigurationError",
                        Message = "Some jwt values are not configured correctly",
                        MessagePriority = MessagePriority.Error
                    }
                }
            };
        }

        public static JwtAuthenticationResult RegisterError(IEnumerable<IdentityError> errors)
        {
            var jwtResult = new JwtAuthenticationResult();
            foreach (var error in errors)
            {
                jwtResult.Messages.Add(new ServiceMessage
                {
                    Code = error.Code,
                    Message = error.Description,
                    MessagePriority = MessagePriority.Error
                });
            }

            return jwtResult;
        }
    }
}
