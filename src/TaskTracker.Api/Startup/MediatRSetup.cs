using System.Reflection;
using FluentValidation;
using MediatR;
using TaskTracker.Application.Behaviors;
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
    
		services.AddValidatorsFromAssembly(typeof(Result<>).Assembly);
		
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		return services;
	}
}