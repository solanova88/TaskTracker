using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TaskTracker.Application.Common.Models;
using TaskTracker.Application.Dtos.Common;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Interfaces.Cookies;
using TaskTracker.Infrastructure.Settings;

namespace TaskTracker.Infrastructure.Services.Cookies;

public class CookieManagementService : ICookieManagementService
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IJwtService _jwtService;
	private readonly IOptions<AuthSettings> _authSettings;

	public CookieManagementService(IHttpContextAccessor httpContextAccessor, IJwtService jwtService,
		IOptions<AuthSettings> authSettings)
	{
		_httpContextAccessor = httpContextAccessor;
		_jwtService = jwtService;
		_authSettings = authSettings;
	}

	public bool AppendCookie(string userName)
	{
		if (_httpContextAccessor.HttpContext == null)
			throw new InvalidOperationException("Ошибка при при получении доступа к HttpContext");
		
		var cookiePayload = GetCookiePayload(userName);
		
		_httpContextAccessor.HttpContext.Response.Cookies.Append("access_token", cookiePayload.ResultData.Jwt,
			cookiePayload.ResultData.CookieOptions);
		
		return true;
	}

	private Result<CookiePayload> GetCookiePayload(string userName)
	{
		var token = _jwtService.GenerateJwt(userName);
		var cookieOptions = GetCookieOptions();
		
		return Result<CookiePayload>.Success(new CookiePayload(token, cookieOptions));
	}
	
	private CookieOptions GetCookieOptions()
	{
		return new CookieOptions
		{
			IsEssential = _authSettings.Value.IsEssential,
			HttpOnly = _authSettings.Value.HttpOnly,
			Secure = _authSettings.Value.Secure,
			Expires = DateTimeOffset.UtcNow.AddDays(_authSettings.Value.LifetimeInDays)
		};
	}
}