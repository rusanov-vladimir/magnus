namespace Magnus.Business.Domain.Mappings.DynamicFieldsMap
{
	using DynamicFields;

	public class DynamicFieldMap :Map<DynamicField>
	{
		public DynamicFieldMap()
		{
			HasRequired(x => x.Configuration).WithMany().WillCascadeOnDelete(false);
			HasOptional(x => x.Value).WithRequired().WillCascadeOnDelete(true);
		}
	}
}