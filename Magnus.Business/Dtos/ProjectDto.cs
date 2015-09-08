namespace Magnus.Business.Dtos
{
	using System;
	using System.Collections.Generic;
	using Domain;
	using DynamicFields;

	public class ProjectDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<TeamDto> Teams { get; set; }
		public string WorkingDirectory { get; set; }
		public StateType State { get; set; }
		public virtual ICollection<DynamicFieldTemplateDto> FieldTemplates { get; set; }
	}
}