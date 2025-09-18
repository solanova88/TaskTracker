using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Interfaces.Common;
using TaskTracker.Application.Interfaces.Contexts;
using TaskTracker.Application.Interfaces.Cookies;
using TaskTracker.Infrastructure.Services.Auth;
using TaskTracker.Infrastructure.Services.Common;
using TaskTracker.Infrastructure.Services.Contexts;
using TaskTracker.Infrastructure.Services.Cookies;

namespace TaskTracker.Api.Startup;

public static class ServicesSetup
{
	public static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		services.AddScoped<ICookieManagementService, CookieManagementService>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<ICurrentJwtContext, CurrentJwtContext>();
		
		services.AddTransient<IJwtService, JwtService>();
		services.AddTransient<IDateTime, DateTimeService>();
		
		services.AddHttpContextAccessor();
		
		return services;
	}
}