using TaskTracker.Infrastructure.Settings;

namespace TaskTracker.Api.Startup;

public static class SettingsSetup
{
	public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<AuthSettings>(configuration.GetSection(AuthSettings.SectionName));
		
		return services;
	}
}