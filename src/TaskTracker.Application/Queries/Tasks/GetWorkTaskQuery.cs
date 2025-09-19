using MediatR;
using TaskTracker.Application.Common.Models;
using TaskTracker.Application.Dtos.Tasks;

namespace TaskTracker.Application.Queries.Tasks;

public record GetWorkTaskQuery(Guid Id) : IRequest<Result<WorkTaskDto?>>;