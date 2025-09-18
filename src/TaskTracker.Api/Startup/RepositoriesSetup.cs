using TaskTracker.Application.Interfaces.DataAccess;
using TaskTracker.Application.Interfaces.DataAccess.Repositories;
using TaskTracker.Infrastructure.ApplicationDependencies.DataAccess;
using TaskTracker.Infrastructure.ApplicationDependencies.DataAccess.Repositories;

namespace TaskTracker.Api.Startup;

public static class RepositoriesSetup
{
	public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		
		services.AddScoped<IWorkTaskRepository, WorkTaskRepositoryEF>();

		return services;
	}
}