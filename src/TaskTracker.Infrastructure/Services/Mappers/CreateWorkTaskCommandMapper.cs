using TaskTracker.Application.Dtos.Common;
using TaskTracker.Application.Interfaces.Mappings;
using TaskTracker.Domain.Models.Tasks;

namespace TaskTracker.Infrastructure.Services.Mappers;

public class CreateWorkTaskCommandMapper : IMapper<CreateWorkTaskCommandContext, WorkTask>
{
	public WorkTask Map(CreateWorkTaskCommandContext context)
	{
		return new WorkTask
		{
			Id = Guid.NewGuid(),
			Title = context.Command.Title,
			Description = context.Command.Description,
			Author = context.UserName,
			Assignee = context.Command.Assignee,
			Status = context.Command.Status,
			Priority = context.Command.Priority,
			ParentTaskId = context.Command.ParentTaskId,
			RelatedTo = context.Command.RelatedTaskIds?
				.Select(id => new WorkTaskRelation
				{
					WorkTaskId = Guid.NewGuid(),
					RelatedWorkTaskId = id
				})
				.ToList() ?? []
		};
	}
}