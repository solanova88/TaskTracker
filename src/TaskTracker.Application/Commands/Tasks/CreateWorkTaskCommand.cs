using MediatR;
using TaskTracker.Application.Common.Models;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Commands.Tasks;

public record CreateWorkTaskCommand : IRequest<Result<Guid>>
{
	public string Title { get; init; }
	public string Description { get; init; }
	public string? Assignee { get; init; }
	public WorkTaskStatus Status { get; init; }
	public WorkTaskPriority Priority { get; init; }
	public Guid? ParentTaskId { get; init; }
	public ICollection<Guid>? RelatedTaskIds { get; init; } = new List<Guid>();
}