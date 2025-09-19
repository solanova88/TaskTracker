using System.IdentityModel.Tokens.Jwt;
using TaskTracker.Application.Interfaces.Contexts;
using TaskTracker.Application.Interfaces.Users;

namespace TaskTracker.Infrastructure.Services.Users;

public class CurrentUserService :  ICurrentUserService
{
	
	private readonly ICurrentJwtContext  _currentJwtContext;

	public CurrentUserService(ICurrentJwtContext currentJwtContext)
	{
		_currentJwtContext = currentJwtContext;
	}

	public string GetUserName()
	{
		var jwt = _currentJwtContext.Jwt;
		if (string.IsNullOrEmpty(jwt)) throw new ArgumentNullException(nameof(jwt));
		
		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.ReadJwtToken(jwt);
		
		var userNameClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);
		if (userNameClaim == null) throw new InvalidOperationException("Ошибка при получении Name из jwt");
		
		return userNameClaim.Value;
	}
}