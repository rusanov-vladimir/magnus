namespace Magnus.Business.Domain.Interfaces
{
	public interface INamedEntity : IEntity
	{
		string Name { get; set; }
	}
}