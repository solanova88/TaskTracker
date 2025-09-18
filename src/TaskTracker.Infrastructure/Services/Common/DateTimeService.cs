using TaskTracker.Application.Interfaces.Common;

namespace TaskTracker.Infrastructure.Services.Common;

public class DateTimeService : IDateTime
{
	public DateTime UtcNow => DateTime.UtcNow;
}