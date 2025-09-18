using System.ComponentModel.DataAnnotations.Schema;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Models.Base;

namespace TaskTracker.Domain.Models.Tasks;

public class WorkTask : BaseEntity
{
	public string Title { get; set; }
	public string Description { get; set; }
	public string Author { get; set; }
	public string? Assignee { get; set; }
	public WorkTaskStatus Status { get; set; }
	public WorkTaskPriority Priority { get; set; }
	public Guid? ParentTaskId { get; set; }
	[ForeignKey(nameof(ParentTaskId))]
	public virtual WorkTask ParentWorkTask { get; set; }
	protected virtual ICollection<WorkTask> Subtasks { get; set; } = new List<WorkTask>();
	public virtual ICollection<WorkTaskRelation> RelatedTo { get; set; } = new List<WorkTaskRelation>();
	public virtual ICollection<WorkTaskRelation> RelatedFrom { get; set; } = new List<WorkTaskRelation>();
	public bool HasSubtasks => Subtasks.Count > 0;
	public bool IsSubtask => ParentTaskId.HasValue;
}