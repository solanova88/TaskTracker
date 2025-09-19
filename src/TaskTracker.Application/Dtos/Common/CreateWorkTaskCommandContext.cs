using TaskTracker.Application.Commands.Tasks;

namespace TaskTracker.Application.Dtos.Common;

public record CreateWorkTaskCommandContext(CreateWorkTaskCommand Command, string UserName);