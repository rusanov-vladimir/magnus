namespace Magnus.Business.Domain.Mappings
{
	public class ProjectMap:Map<Project>
	{
		public ProjectMap()
		{
			HasMany(x => x.FieldTemplates).WithMany();
			Property(x => x.Name).HasMaxLength(128);
			Property(x => x.WorkingDirectory).IsRequired();
			Property(x => x.State).IsRequired();
		}
	}
}