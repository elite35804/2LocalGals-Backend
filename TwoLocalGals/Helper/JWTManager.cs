using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtManager
{
    private static string Secret = Convert.ToBase64String(Encoding.UTF8.GetBytes("@#2LocalGals1qaz!QAZ"));

    public static string GenerateToken(string username, int contractorID, int access)
    {
        var symmetricKey = Convert.FromBase64String(Secret);
        var tokenHandler = new JwtSecurityTokenHandler();
        int expireMinutes = 60;
        var now = DateTime.UtcNow;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim("access", $"{access}"),
            new Claim("contractorID", $"{contractorID}"),
            }),
            Expires = now.AddMinutes(expireMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature),

        };

        var stoken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(stoken);

        return token;
    }
}
