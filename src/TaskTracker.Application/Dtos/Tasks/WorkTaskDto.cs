using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Dtos.Tasks;

public record WorkTaskDto
{
	public Guid Id { get; init; }
	public string Title { get; init; }
	public string Description { get; init; }
	public string Author { get; init; }
	public string? Assignee { get; init; }
	public WorkTaskStatus Status { get; init; }
	public WorkTaskPriority Priority { get; init; }
	public Guid? ParentTaskId { get; init; }
	public DateTime CreatedAt { get; init; }
	public DateTime UpdatedAt { get; init; }
	public bool HasSubtasks { get; init; }
	public bool IsSubtask { get; init; }
	public ICollection<WorkTaskDto> Subtasks { get; init; } = new List<WorkTaskDto>();
	public ICollection<Guid> RelatedTaskIds { get; init; } = new List<Guid>();
	public ICollection<Guid> RelatedFromTaskIds { get; init; } = new List<Guid>();
}