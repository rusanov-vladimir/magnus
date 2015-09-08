namespace Magnus.Business.Dtos.DynamicFields
{
	using System;

	public abstract class ValueDto
	{
		public Guid Id { get; set; }
		public DynamicFieldDto Parent { get; set; }

	}
}