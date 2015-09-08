namespace Magnus.Business.Dtos
{
	using System;
	using System.Collections.Generic;
	using DynamicFields;

	public class DocumentDto
	{
		public string Name { get; set; }
		public ICollection<DynamicFieldDto> Fields { get; set; }
		public Guid Id { get; set; }
	}
}