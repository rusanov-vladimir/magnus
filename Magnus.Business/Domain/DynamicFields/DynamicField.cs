namespace Magnus.Business.Domain.DynamicFields
{
	using System;

	public class DynamicField:Entity
	{
		public virtual DynamicFieldTemplate Configuration { get; set; }
		public virtual DynamicFieldValue Value { get;  set; }

		public void CreateValue()
		{
			switch (Configuration.Type)
			{
				case DynamicFieldType.Integer:
					Value = new IntegerValue();
					break;
				case DynamicFieldType.String:
					Value = new StringValue();
					break;
				case DynamicFieldType.Bool:
					Value = new BoolValue();
					break;
				case DynamicFieldType.Alphanumeric:
					Value = new StringValue();
					break;
				case DynamicFieldType.DateTime:
					Value = new DateTimeValue();
					break;
				case DynamicFieldType.Double:
					Value = new DoubleValue();
					break;
				default:
					throw new IndexOutOfRangeException("Unexpected field type");
			}
		}

		public object GetValue()
		{
			if (Value == null)
				return null;
			switch (Configuration.Type)
			{
				case DynamicFieldType.Integer:
					return ((IntegerValue) Value).Value;

				case DynamicFieldType.String:
					return ((StringValue) Value).Value;

				case DynamicFieldType.Bool:
					return ((BoolValue) Value).Value;

				case DynamicFieldType.Alphanumeric:
					return ((StringValue) Value).Value;

				case DynamicFieldType.DateTime:
					return ((DateTimeValue) Value).Value;

				case DynamicFieldType.Double:
					return ((DoubleValue) Value).Value;

				default:
					throw new IndexOutOfRangeException("Unexpected field type");
			}
		}

		public void SetValue(object newValue)
		{
			if (Value == null)
				CreateValue();

			switch (Configuration.Type)
			{
				case DynamicFieldType.Integer:
					((IntegerValue) Value).Value = (int?) newValue;
					break;

				case DynamicFieldType.String:
					((StringValue) Value).Value = (string) newValue;
					break;

				case DynamicFieldType.Bool:
					((BoolValue) Value).Value = (bool?) newValue;
					break;

				case DynamicFieldType.Alphanumeric:
					((StringValue) Value).Value = (string) newValue;
					break;

				case DynamicFieldType.DateTime:
					((DateTimeValue) Value).Value = (DateTime?) newValue;
					break;

				case DynamicFieldType.Double:
					((DoubleValue) Value).Value = (double?) newValue;
					break;

				default:
					throw new IndexOutOfRangeException("Unexpected field type");
			}
		}
	}
}