namespace Magnus.Business.Domain.Mappings
{
	internal class TaskMap : Map<Task>
	{
		private TaskMap()
		{
			HasRequired(x => x.Project).WithMany().WillCascadeOnDelete(true);
			HasRequired(x => x.CurrentUser).WithMany();
			Property(x => x.Name).IsRequired().HasMaxLength(128);
			HasMany(x => x.Documents).WithRequired();
			Property(x => x.CurrentDocumentId);
		}
	}
}
