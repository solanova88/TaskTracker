using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Infrastructure.Settings;

namespace TaskTracker.Infrastructure.Services.Auth;

public class JwtService : IJwtService
{
	private readonly string _jwtSigningKey;
	private readonly int _jwtLifetimeInDays;

	public JwtService(IOptions<AuthSettings> authSettings)
	{
		_jwtSigningKey = authSettings.Value.JwtSigningKey;
		_jwtLifetimeInDays = authSettings.Value.LifetimeInDays;
	}

	public string GenerateJwt(string userName)
	{
		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Name, userName),
		};
		
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSigningKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var expires = DateTime.UtcNow.AddDays(_jwtLifetimeInDays);
		
		var token = new JwtSecurityToken(
			claims: claims,
			expires: expires,
			signingCredentials: credentials
		);
		
		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}