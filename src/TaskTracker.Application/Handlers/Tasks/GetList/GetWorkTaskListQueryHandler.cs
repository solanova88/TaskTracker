using MediatR;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.Common.Models;
using TaskTracker.Application.Dtos.Tasks;
using TaskTracker.Application.Interfaces.DataAccess;
using TaskTracker.Application.Interfaces.Mappings;
using TaskTracker.Application.Queries.Tasks;
using TaskTracker.Domain.Models.Tasks;

namespace TaskTracker.Application.Handlers.Tasks.GetList;

public class GetWorkTaskListQueryHandler : IRequestHandler<GetWorkTaskListQuery, Result<IEnumerable<WorkTaskDto>>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<GetWorkTaskListQueryHandler> _logger;
	private readonly IMapper<WorkTask, WorkTaskDto> _mapper;

	public GetWorkTaskListQueryHandler(
		IUnitOfWork unitOfWork,
		ILogger<GetWorkTaskListQueryHandler> logger,
		IMapper<WorkTask, WorkTaskDto> mapper)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_mapper = mapper;
	}

	public async Task<Result<IEnumerable<WorkTaskDto>>> Handle(GetWorkTaskListQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var skip = (request.Page - 1) * request.PageSize;
			var tasks = await _unitOfWork.Tasks.GetListWithSubtasksAndRelationsAsync(
				author: request.Author,
				assignee: request.Assignee,
				status: request.Status,
				priority: request.Priority,
				hasParent: request.HasParent,
				skip: skip,
				take: request.PageSize
			);

			var dtos = tasks.Select(t => _mapper.Map(t)).ToList();
			return Result<IEnumerable<WorkTaskDto>>.Success(dtos);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error retrieving work task list");
			return Result<IEnumerable<WorkTaskDto>>.Failure(ex.Message);
		}
	}
}