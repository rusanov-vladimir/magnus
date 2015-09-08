namespace Magnus.Business.Domain.Mappings.DynamicFieldsMap
{
	using DynamicFields;

	public class BoolValueMap : Map<BoolValue>
	{
		public BoolValueMap()
		{
			Property(x => x.Value).HasColumnName("BoolValue");
		}
	}
}