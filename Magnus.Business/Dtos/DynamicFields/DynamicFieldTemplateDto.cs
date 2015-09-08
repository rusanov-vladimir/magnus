namespace Magnus.Business.Dtos.DynamicFields
{
	using System;
	using Domain.DynamicFields;

	public class DynamicFieldTemplateDto
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public DynamicFieldType Type { get; set; }
		public bool IsEnabled { get; set; }
		public string Label { get; set; }
		public int Weight { get; set; }
		public int? Length { get; set; }
	}
}