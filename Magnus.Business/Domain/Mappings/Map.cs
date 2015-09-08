namespace Magnus.Business.Domain.Mappings
{
	using System.Data.Entity.ModelConfiguration;

	public class Map<T> : EntityTypeConfiguration<T>, IMap where T : class
	{
		 
	}
}