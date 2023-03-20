using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace MyCoreBanking.API.Helpers;

public static class JwtHelper
{
    public static string GenerateJwtToken(string secretKey, string issuer, string audience, int expireInMinutes, string userId)
    {
        // Create the claims for the token
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        // Create the signing credentials
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create the JWT token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireInMinutes),
            signingCredentials: creds
        );

        // Return the JWT token as a string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static Guid ValidateAndThrow(string secretKey, string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = creds.Key,
            ValidateIssuer = true,
            ValidIssuer = "localhost",
            ValidateAudience = true,
            ValidAudience = "localhost",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var claims = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return new Guid(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}
