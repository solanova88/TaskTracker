using FluentValidation;
using TaskTracker.Application.Commands.Tasks;
using TaskTracker.Application.Interfaces.DataAccess;

namespace TaskTracker.Application.Validators;

public class CreateWorkTaskCommandValidator 
	: AbstractValidator<CreateWorkTaskCommand>
{
	private readonly IUnitOfWork _unitOfWork;

	public CreateWorkTaskCommandValidator(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;

		RuleFor(x => x.Title)
			.NotEmpty().WithMessage("Title is required");

		RuleFor(x => x.Status)
			.IsInEnum();

		RuleFor(x => x.Priority)
			.IsInEnum();

		RuleFor(x => x.ParentTaskId)
			.MustAsync(async (parentTaskId, _) =>
			{
				if (!parentTaskId.HasValue) return true;

				var task = await _unitOfWork.Tasks.GetByIdAsync(parentTaskId.Value);
				return task != null;
			})
			.WithMessage("Parent task with given ID does not exist");
	}
}