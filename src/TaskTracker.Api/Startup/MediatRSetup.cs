using System.Reflection;
using TaskTracker.Application.Common.Models;

namespace TaskTracker.Api.Startup;

public static class MediatRSetup
{
	public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
	{
		services.AddMediatR(conf =>
		{
			conf.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(Result<>))!);
		});

		return services;
	}
}