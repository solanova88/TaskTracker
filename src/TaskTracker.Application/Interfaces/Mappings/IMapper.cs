namespace TaskTracker.Application.Interfaces.Mappings;

public interface IMapper<in TDomain, out TDto>
{
	TDto Map(TDomain model);
}