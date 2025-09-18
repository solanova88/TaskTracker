namespace TaskTracker.Application.Interfaces.Common;

public interface IDateTime
{
	/// <summary>
	/// Текущая дата UTC
	/// </summary>
	DateTime UtcNow { get; }
}