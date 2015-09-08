namespace Magnus.Business.Dtos.DynamicFields
{
	using System;
	using Domain.DynamicFields;

	public class DynamicFieldDto
	{
		public Guid Id { get; set; }
		public DynamicFieldTemplateDto Configuration { get; set; }
		public ValueDto Value { get; set; }

		public virtual void CreateValue()
		{
			switch (Configuration.Type)
			{
				case DynamicFieldType.Integer:
					Value = new IntegerValueDto();
					break;
				case DynamicFieldType.String:
					Value = new StringValueDto();
					break;
				case DynamicFieldType.Bool:
					Value = new BoolValueDto();
					break;
				case DynamicFieldType.Alphanumeric:
					Value = new StringValueDto();
					break;
				case DynamicFieldType.DateTime:
					Value = new DateTimeValueDto();
					break;
				case DynamicFieldType.Double:
					Value = new DoubleValueDto();
					break;
				default:
					throw new IndexOutOfRangeException("Unexpected field type");
			}
			Value.Parent = this;
		}

		public virtual object GetValue()
		{
			if (Value == null)
				return null;
			switch (Configuration.Type)
			{
				case DynamicFieldType.Integer:
					return ((IntegerValueDto)Value).Value;

				case DynamicFieldType.String:
					return ((StringValueDto)Value).Value;

				case DynamicFieldType.Bool:
					return ((BoolValueDto)Value).Value;

				case DynamicFieldType.Alphanumeric:
					return ((StringValueDto)Value).Value;

				case DynamicFieldType.DateTime:
					return ((DateTimeValueDto)Value).Value;

				case DynamicFieldType.Double:
					return ((DoubleValueDto)Value).Value;

				default:
					throw new IndexOutOfRangeException("Unexpected field type");
			}
		}

		public virtual void SetValue(object newValue)
		{
			if (Value == null)
				CreateValue();

			switch (Configuration.Type)
			{
				case DynamicFieldType.Integer:
					((IntegerValueDto)Value).Value = (int)newValue;
					break;

				case DynamicFieldType.String:
					((StringValueDto)Value).Value = (string)newValue;
					break;

				case DynamicFieldType.Bool:
					((BoolValueDto)Value).Value = (bool)newValue;
					break;

				case DynamicFieldType.Alphanumeric:
					((StringValueDto)Value).Value = (string)newValue;
					break;

				case DynamicFieldType.DateTime:
					((DateTimeValueDto)Value).Value = (DateTime)newValue;
					break;

				case DynamicFieldType.Double:
					((DoubleValueDto)Value).Value = (double)newValue;
					break;

				default:
					throw new IndexOutOfRangeException("Unexpected field type");
			}
		}

	}
}