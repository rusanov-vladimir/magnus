namespace Magnus.Business.Domain.Mappings.DynamicFieldsMap
{
	using DynamicFields;

	public class DateTimeValueMap : Map<DateTimeValue>
	{
		public DateTimeValueMap()
		{
			Property(x => x.Value).HasColumnName("DateTimeValue");
		}
	}
}