namespace Magnus.Business.Domain.Mappings.DynamicFieldsMap
{
	using DynamicFields;

	public class DoubleValueMap : Map<DoubleValue>
	{
		public DoubleValueMap()
		{
			Property(x => x.Value).HasColumnName("DoubleValue");
		}
	}
}