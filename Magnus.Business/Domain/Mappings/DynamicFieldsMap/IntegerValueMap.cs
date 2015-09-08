namespace Magnus.Business.Domain.Mappings.DynamicFieldsMap
{
	using DynamicFields;

	public class IntegerValueMap : Map<IntegerValue>
	{
		public IntegerValueMap()
		{
			Property(x => x.Value).HasColumnName("IntegerValue");
		}
	}
}