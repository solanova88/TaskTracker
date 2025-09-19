using MediatR;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.Commands.Tasks;
using TaskTracker.Application.Common.Models;
using TaskTracker.Application.Dtos.Common;
using TaskTracker.Application.Interfaces.DataAccess;
using TaskTracker.Application.Interfaces.Mappings;
using TaskTracker.Application.Interfaces.Users;
using TaskTracker.Domain.Models.Tasks;

namespace TaskTracker.Application.Handlers.Tasks.Create;

public class CreateWorkTaskCommandHandler : IRequestHandler<CreateWorkTaskCommand, Result<Guid>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentUserService _currentUserService;
	private readonly ILogger<CreateWorkTaskCommandHandler> _logger;
	private readonly IMapper<CreateWorkTaskCommandContext, WorkTask> _createWorkTaskCommandMapper;

	public CreateWorkTaskCommandHandler(
		IUnitOfWork unitOfWork,
		ICurrentUserService currentUserService,
		ILogger<CreateWorkTaskCommandHandler> logger,
		IMapper<CreateWorkTaskCommandContext, WorkTask> createWorkTaskCommandMapper)
	{
		_unitOfWork = unitOfWork;
		_currentUserService = currentUserService;
		_logger = logger;
		_createWorkTaskCommandMapper = createWorkTaskCommandMapper;
	}

	public async Task<Result<Guid>> Handle(CreateWorkTaskCommand request, CancellationToken cancellationToken)
	{
		try
		{
			await _unitOfWork.BeginTransactionAsync(cancellationToken);
			
			var userName = _currentUserService.GetUserName();

			_logger.LogInformation("Creating work task '{Title}' for user '{User}'", request.Title, userName);

			var workTask = _createWorkTaskCommandMapper.Map(new CreateWorkTaskCommandContext(request, userName));

			await _unitOfWork.Tasks.AddAsync(workTask);
			
			await _unitOfWork.CommitTransactionAsync();

			_logger.LogInformation("Work task created successfully with ID {TaskId}", workTask.Id);

			return Result<Guid>.Success(workTask.Id);
		}
		catch (Exception ex)
		{
			await _unitOfWork.RollbackTransactionAsync();
			
			_logger.LogError(ex, "An error occurred while creating work task");
			
			return Result<Guid>.Failure(ex.Message);
		}
	}
}