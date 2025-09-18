namespace TaskTracker.Domain.Common;

public interface IEntity
{
	/// <summary>
	/// Идентификатор сущности
	/// </summary>
	Guid Id { get; }
}