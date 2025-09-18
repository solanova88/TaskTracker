using Newtonsoft.Json.Converters;
using TaskTracker.Api.Filters;

namespace TaskTracker.Api.Startup;

public static class ControllersSetup
{
	public static IServiceCollection ConfigureControllers(this IServiceCollection services)
	{
		services
			.AddControllers(options => { options.Filters.Add<GlobalExceptionFilter>(); })
			.AddNewtonsoftJson(options => { options.SerializerSettings.Converters.Add(new StringEnumConverter()); });
		
		services.AddProblemDetails();

		return services;
	}
}