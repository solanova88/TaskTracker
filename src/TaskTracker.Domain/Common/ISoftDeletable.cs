namespace TaskTracker.Domain.Common;

public interface ISoftDeletable
{
	/// <summary>
	/// Дата удаления записи
	/// </summary>
	public DateTime DeletedDate { get; }
	/// <summary>
	/// Признак удаления записи
	/// </summary>
	public bool IsDeleted { get; }
}