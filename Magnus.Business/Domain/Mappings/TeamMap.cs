namespace Magnus.Business.Domain.Mappings
{
	public class TeamMap : Map<Team>
	{
		public TeamMap()
		{
			HasMany(x => x.Users).WithOptional(x=>x.Team);
			Property(x => x.Name).HasMaxLength(128);
			HasRequired(x=>x.Project).WithMany(x=>x.Teams).WillCascadeOnDelete(true);
		} 
	}
}