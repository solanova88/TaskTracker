using MediatR;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.Common.Models;
using TaskTracker.Application.Dtos.Tasks;
using TaskTracker.Application.Interfaces.DataAccess;
using TaskTracker.Application.Interfaces.Mappings;
using TaskTracker.Application.Queries.Tasks;
using TaskTracker.Domain.Models.Tasks;

namespace TaskTracker.Application.Handlers.Tasks.Get;

public class GetWorkTaskQueryHandler : IRequestHandler<GetWorkTaskQuery, Result<WorkTaskDto?>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetWorkTaskQueryHandler> _logger;
    private readonly IMapper<WorkTask, WorkTaskDto> _workTaskDtoMapper;

    public GetWorkTaskQueryHandler(ILogger<GetWorkTaskQueryHandler> logger, IUnitOfWork unitOfWork, IMapper<WorkTask, WorkTaskDto> workTaskDtoMapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _workTaskDtoMapper = workTaskDtoMapper;
    }

    public async Task<Result<WorkTaskDto?>> Handle(GetWorkTaskQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _unitOfWork.Tasks.GetWithSubtasksAndRelationsAsync(request.Id, readOnly: true);

            if (task is null)
            {
                _logger.LogWarning("Task with Id {TaskId} not found", request.Id);
                return Result<WorkTaskDto?>.Failure($"Task with Id {request.Id} not found");
            }

            var dto = _workTaskDtoMapper.Map(task);
            
            return Result<WorkTaskDto?>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task {TaskId}", request.Id);
            return Result<WorkTaskDto?>.Failure(ex.Message);
        }
    }
}