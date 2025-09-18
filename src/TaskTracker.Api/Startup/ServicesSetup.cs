using TaskTracker.Application.Interfaces.Common;
using TaskTracker.Application.Interfaces.Contexts;
using TaskTracker.Infrastructure.Services.Common;
using TaskTracker.Infrastructure.Services.Contexts;

namespace TaskTracker.Api.Startup;

public static class ServicesSetup
{
	public static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		services.AddScoped<ICurrentJwtContext, CurrentJwtContext>();
		
		services.AddTransient<IDateTime, DateTimeService>();
		
		return services;
	}
}