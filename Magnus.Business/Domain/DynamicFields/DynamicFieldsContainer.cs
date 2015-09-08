namespace Magnus.Business.Domain.DynamicFields
{
	using System.Collections.Generic;
	using Interfaces;

	public class DynamicFieldsContainer :Entity, IDynamicFieldsContainer
	{
		public virtual List<DynamicField> Fields { get; set; }

		public DynamicFieldsContainer()
		{
			Fields = new List<DynamicField>();
		}
	}
}