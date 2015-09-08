namespace Magnus.Business.Domain.Mappings.DynamicFieldsMap
{
	using DynamicFields;

	public class StringValueMap : Map<StringValue>
	{
		public StringValueMap()
		{
			Property(x => x.Value).HasColumnName("StringValue");
		}
	}
}