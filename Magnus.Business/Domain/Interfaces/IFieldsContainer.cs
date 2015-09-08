namespace Magnus.Business.Domain.Interfaces
{
	using System.Collections.Generic;
	using DynamicFields;

	public interface IDynamicFieldsContainer
	{
		List<DynamicField> Fields { get; set; }
	}
}