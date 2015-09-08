namespace Magnus.Business.Domain
{
	using System.Collections.Generic;
	using DynamicFields;
	using Interfaces;

	public class Project : DynamicFieldsContainer, INamedEntity
	{
		public string Name { get; set; }
		public virtual ICollection<Team> Teams { get; set; }
		public string WorkingDirectory { get; set; }
		public StateType State { get; set; }
		public virtual ICollection<DynamicFieldTemplate> FieldTemplates { get; set; }

		public Project()
		{
			Teams = new List<Team>();
			FieldTemplates = new List<DynamicFieldTemplate>();
		}
	}
}