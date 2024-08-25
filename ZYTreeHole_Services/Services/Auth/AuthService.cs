using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZYTreeHole_Models.ViewModels.Auth;
using ZYTreeHole_Models.ViewModels.Config;

namespace ZYTreeHole_Services.Services;

public class AuthService
{
    private readonly Auth _auth;
    public AuthService(IOptions<Auth> options) {
        _auth = options.Value;
    }

    public async Task<LoginUser?> GetUserByName(string userNamer)
    {
        return await Task.Run(() =>
        {
            if (userNamer == "zy")
                return new LoginUser { UserName = "zy", Password = "123" };
            else
                return null;
        });
    }
    public LoginToken GenerateLoginToken(string userId, string userName) {
        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, userId), // User.Identity.Name
            new(JwtRegisteredClaimNames.GivenName, userName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_auth.Jwt.Key));
        var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(
            issuer: _auth.Jwt.Issuer,
            audience: _auth.Jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: signCredential);

        return new LoginToken {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            Expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
        };
    }
}