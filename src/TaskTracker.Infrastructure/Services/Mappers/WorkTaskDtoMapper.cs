using TaskTracker.Application.Dtos.Tasks;
using TaskTracker.Application.Interfaces.Mappings;
using TaskTracker.Domain.Models.Tasks;

namespace TaskTracker.Infrastructure.Services.Mappers;

public class WorkTaskDtoMapper : IMapper<WorkTask, WorkTaskDto>
{
	public WorkTaskDto Map(WorkTask model)
	{
		return new WorkTaskDto
		{
			Id = model.Id,
			Title = model.Title,
			Description = model.Description,
			Author = model.Author,
			Assignee = model.Assignee,
			Status = model.Status,
			Priority = model.Priority,
			ParentTaskId = model.ParentTaskId,
			CreatedAt = model.CreatedDate,
			UpdatedAt = model.UpdatedDate,
			HasSubtasks = model.Subtasks.Count != 0,
			IsSubtask = model.ParentTaskId.HasValue,
			Subtasks = model.Subtasks.Select(Map).ToList(),
			RelatedTaskIds = model.RelatedTo.Select(r => r.RelatedWorkTaskId).ToList(),
			RelatedFromTaskIds = model.RelatedFrom.Select(r => r.WorkTaskId).ToList()
		};
	}
}