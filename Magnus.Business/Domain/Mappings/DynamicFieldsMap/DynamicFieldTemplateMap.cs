namespace Magnus.Business.Domain.Mappings.DynamicFieldsMap
{
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Infrastructure.Annotations;
	using DynamicFields;

	public class DynamicFieldTemplateMap : Map<DynamicFieldTemplate>
	{
		public DynamicFieldTemplateMap()
		{
			Property(x => x.Code).HasMaxLength(10).HasColumnAnnotation(IndexAnnotation.AnnotationName,
				new IndexAnnotation(new IndexAttribute("IX_UQ_Code")
				{
					IsUnique = true
				})).IsRequired();
			Property(x => x.IsEnabled).IsRequired();
			Property(x => x.Label).IsOptional();
		}
	}
}