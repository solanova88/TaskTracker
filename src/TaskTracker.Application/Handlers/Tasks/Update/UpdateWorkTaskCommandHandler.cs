using MediatR;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.Commands.Tasks;
using TaskTracker.Application.Common.Models;
using TaskTracker.Application.Interfaces.DataAccess;
using TaskTracker.Domain.Models.Tasks;

namespace TaskTracker.Application.Handlers.Tasks.Update;

public class UpdateWorkTaskCommandHandler : IRequestHandler<UpdateWorkTaskCommand, Result<bool>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<UpdateWorkTaskCommandHandler> _logger;

	public UpdateWorkTaskCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateWorkTaskCommandHandler> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public async Task<Result<bool>> Handle(UpdateWorkTaskCommand request, CancellationToken cancellationToken)
	{
		try
		{
			await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
			var task = await _unitOfWork.Tasks.GetByIdAsync(request.Id);
			if (task is null)
				return Result<bool>.Failure($"Task with Id {request.Id} not found");

			UpdateTaskProperties(task, request);

			await _unitOfWork.CommitTransactionAsync();

			_logger.LogInformation("Task {TaskId} updated successfully", task.Id);

			return Result<bool>.Success(true);
		}
		catch (Exception ex)
		{
			await _unitOfWork.RollbackTransactionAsync();
			_logger.LogError(ex, "Error updating task {TaskId}", request.Id);
			return Result<bool>.Failure(ex.Message);
		}
	}

	private static void UpdateTaskProperties(WorkTask task, UpdateWorkTaskCommand request)
	{
		var properties = typeof(UpdateWorkTaskCommand).GetProperties();
		foreach (var prop in properties)
		{
			if (prop.Name == nameof(UpdateWorkTaskCommand.Id)) 
				continue;

			var value = prop.GetValue(request);
			if (value is null) 
				continue;

			var taskProp = typeof(WorkTask).GetProperty(prop.Name);
			if (taskProp == null || !taskProp.CanWrite) 
				continue;

			taskProp.SetValue(task, value);
		}
	}
}