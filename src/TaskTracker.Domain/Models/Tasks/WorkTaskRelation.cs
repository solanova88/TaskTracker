namespace TaskTracker.Domain.Models.Tasks;

public class WorkTaskRelation
{
	public Guid WorkTaskId { get; set; }
	public WorkTask WorkTask { get; set; }

	public Guid RelatedWorkTaskId { get; set; }
	public WorkTask RelatedWorkTask { get; set; }
}