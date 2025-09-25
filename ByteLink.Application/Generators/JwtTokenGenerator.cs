using ByteLink.Application.Mediator.Commands;
using ByteLink.Domain.Generators;
using ByteLink.Domain.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ByteLink.Application.Generators;

public class JwtTokenGenerator(
    ByteLinkAuthSettings authSettings
) : IGenerator<string, string>
{
    public string Generate(string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, email)
        };

        var token = new JwtSecurityToken(
            issuer: authSettings.Issuer,
            audience: authSettings.Issuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            notBefore: DateTime.Now,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}