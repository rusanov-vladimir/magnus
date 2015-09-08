namespace Magnus.Business.Domain.DynamicFields
{
	public class DynamicFieldTemplate :Entity
	{
		public string Code { get; set; }
		public DynamicFieldType Type { get; set; }
		public bool IsEnabled { get; set; }
		public string Label { get; set; }
		public int Weight { get; set; }
		public int? Length { get; set; }
	}
}