using MediatR;
using TaskTracker.Application.Common.Models;
using TaskTracker.Application.Dtos.Tasks;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Queries.Tasks;

public record GetWorkTaskListQuery : IRequest<Result<IEnumerable<WorkTaskDto>>>
{
	public string? Author { get; init; }
	public string? Assignee { get; init; }
	public WorkTaskStatus? Status { get; init; }
	public WorkTaskPriority? Priority { get; init; }
	public bool? HasParent { get; init; }
	public int Page { get; init; } = 1;
	public int PageSize { get; init; } = 10;
}