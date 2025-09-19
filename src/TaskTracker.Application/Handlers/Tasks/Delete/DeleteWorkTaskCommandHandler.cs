using MediatR;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.Commands.Tasks;
using TaskTracker.Application.Common.Models;
using TaskTracker.Application.Interfaces.DataAccess;

namespace TaskTracker.Application.Handlers.Tasks.Delete;

public class DeleteWorkTaskCommandHandler : IRequestHandler<DeleteWorkTaskCommand, Result<bool>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<DeleteWorkTaskCommandHandler> _logger;

	public DeleteWorkTaskCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteWorkTaskCommandHandler> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public async Task<Result<bool>> Handle(DeleteWorkTaskCommand request, CancellationToken cancellationToken)
	{
		try
		{
			await _unitOfWork.BeginTransactionAsync(cancellationToken);

			var task = await _unitOfWork.Tasks.GetWithRelationsAsync(request.Id);
			if (task is null)
				return Result<bool>.Failure($"Task {request.Id} not found");
			
			task.RelatedTo.Clear();
			task.RelatedFrom.Clear();

			_unitOfWork.Tasks.Remove(task);

			await _unitOfWork.CommitTransactionAsync();

			_logger.LogInformation("Task {TaskId} deleted successfully", task.Id);

			return Result<bool>.Success(true);
		}
		catch (Exception ex)
		{
			await _unitOfWork.RollbackTransactionAsync();
			_logger.LogError(ex, "Error deleting task {TaskId}", request.Id);
			return Result<bool>.Failure(ex.Message);
		}
	}
}