namespace TaskTracker.Domain.Common;

public interface IAuditable
{
	/// <summary>
	/// Дата создания записи
	/// </summary>
	public DateTime CreatedDate { get; }
	/// <summary>
	/// Дата обновления записи
	/// </summary>
	public DateTime UpdatedDate { get; }
}