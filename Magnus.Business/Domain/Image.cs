namespace Magnus.Business.Domain
{
	using System.Collections.Generic;
	using DynamicFields;
	using Interfaces;

	public class Document : DynamicFieldsContainer, INamedEntity
	{
		public string Name { get; set; }
	}
}
