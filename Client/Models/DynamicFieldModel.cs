using System;

namespace Client.Models
{
	using Magnus.Business.Domain.DynamicFields;
	using Magnus.Business.Dtos.DynamicFields;

	public class DynamicFieldModel : DynamicFieldDto
	{
		public bool IsValid;

		public void CreateValue(Action<bool> action)
		{
			switch (Configuration.Type)
			{
				case DynamicFieldType.Integer:
					Value = new IntegerValueModel { ValidateAction = action };
					break;
				case DynamicFieldType.String:
					Value = new StringValueModel { ValidateAction = action };
					break;
				case DynamicFieldType.Bool:
					Value = new BoolValueModel { ValidateAction = action };
					break;
				case DynamicFieldType.Alphanumeric:
					Value = new StringValueModel { ValidateAction = action };
					break;
				case DynamicFieldType.DateTime:
					Value = new DateTimeValueModel { ValidateAction = action };
					break;
				case DynamicFieldType.Double:
					Value = new DoubleValueModel { ValidateAction = action };
					break;
				default:
					throw new IndexOutOfRangeException("Unexpected field type");
			}
			Value.Parent = this;
		}

		public override object GetValue()
		{
			if (Value == null)
				return null;
			switch (Configuration.Type)
			{
				case DynamicFieldType.Integer:
					return ((IntegerValueModel)Value).Value;

				case DynamicFieldType.String:
					return ((StringValueModel)Value).Value;

				case DynamicFieldType.Bool:
					return ((BoolValueModel)Value).Value;

				case DynamicFieldType.Alphanumeric:
					return ((StringValueModel)Value).Value;

				case DynamicFieldType.DateTime:
					return ((DateTimeValueModel)Value).Value;

				case DynamicFieldType.Double:
					return ((DoubleValueModel)Value).Value;

				default:
					throw new IndexOutOfRangeException("Unexpected field type");
			}
		}

		public override void SetValue(object newValue)
		{
			if (Value == null)
				CreateValue();

			switch (Configuration.Type)
			{
				case DynamicFieldType.Integer:
					((IntegerValueModel)Value).Value = (int)newValue;
					break;

				case DynamicFieldType.String:
					((StringValueModel)Value).Value = (string)newValue;
					break;

				case DynamicFieldType.Bool:
					((BoolValueModel)Value).Value = (bool)newValue;
					break;

				case DynamicFieldType.Alphanumeric:
					((StringValueModel)Value).Value = (string)newValue;
					break;

				case DynamicFieldType.DateTime:
					((DateTimeValueModel)Value).Value = (DateTime)newValue;
					break;

				case DynamicFieldType.Double:
					((DoubleValueModel)Value).Value = (double)newValue;
					break;

				default:
					throw new IndexOutOfRangeException("Unexpected field type");
			}
		}
	}
}
