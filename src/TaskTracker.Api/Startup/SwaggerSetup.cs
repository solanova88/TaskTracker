using Microsoft.OpenApi.Models;

namespace TaskTracker.Api.Startup;

public static class SwaggerSetup
{
	public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(opt =>
		{
			opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskTracker.Api", Version = "v1" });
		});
		services.AddSwaggerGenNewtonsoftSupport();

		return services;
	}
}