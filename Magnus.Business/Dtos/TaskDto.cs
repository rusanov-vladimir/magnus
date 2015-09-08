namespace Magnus.Business.Dtos
{
	using System;
	using System.Collections.Generic;

	public class TaskDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public UserDto CurrentUser { get; set; }
		public ProjectDto Project { get; set; }
		public DocumentDto CurrentDocument { get; set; }
		public IList<DocumentDto> Documents { get; set; }
	}
}
