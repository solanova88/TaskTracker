using TaskTracker.Domain.Common;

namespace TaskTracker.Domain.Models.Base;

public abstract class BaseEntity :  IEntity,  IAuditable, ISoftDeletable
{
	/// <inheritdoc />
	public Guid Id { get; set; }
	/// <inheritdoc />
	public DateTime CreatedDate { get; set; }
	/// <inheritdoc />
	public DateTime UpdatedDate { get; set; }
	/// <inheritdoc />
	public DateTime DeletedDate { get; set; }
	/// <inheritdoc />
	public bool IsDeleted { get; set; }
}