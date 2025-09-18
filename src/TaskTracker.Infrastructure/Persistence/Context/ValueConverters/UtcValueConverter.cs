using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TaskTracker.Infrastructure.Persistence.Context.ValueConverters;

internal class UtcValueConverter : ValueConverter<DateTime, DateTime>
{
	public UtcValueConverter()
		: base(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
	{
	}
}