using TaskTracker.Application.Dtos.Common;
using TaskTracker.Application.Dtos.Tasks;
using TaskTracker.Application.Interfaces.Mappings;
using TaskTracker.Domain.Models.Tasks;
using TaskTracker.Infrastructure.Services.Mappers;

namespace TaskTracker.Api.Startup;

public static class MappingSetup
{
	public static IServiceCollection ConfigureMapping(this IServiceCollection services)
	{
		services.AddScoped<IMapper<WorkTask, WorkTaskDto>, WorkTaskDtoMapper>();
		services.AddScoped<IMapper<CreateWorkTaskCommandContext, WorkTask>, CreateWorkTaskCommandMapper>();
		
		return services;
	}
}