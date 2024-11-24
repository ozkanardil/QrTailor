using QrTailor.Application.Features.UserRole.Models;
using QrTailor.Infrastructure.Security.JwtToken;

namespace QrTailor.Application.Features.Auth.Models
{
    public class LoginResponse
    {
        public TokenResult Token { get; set; }
        public List<UserRoleResponse> Roles { get; set; }
    }

    public class TokenResult : AccessToken
    {
        public string refreshToken { get; set; }
    }

}
