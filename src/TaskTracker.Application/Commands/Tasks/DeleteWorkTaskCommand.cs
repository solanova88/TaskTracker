using MediatR;
using TaskTracker.Application.Common.Models;

namespace TaskTracker.Application.Commands.Tasks;

public record DeleteWorkTaskCommand(Guid Id) : IRequest<Result<bool>>;