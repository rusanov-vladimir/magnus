namespace Client.Models
{
	using System;
	using System.ComponentModel;
	using Magnus.Business.Dtos;
	using Magnus.Business.Dtos.DynamicFields;

	class DoubleValueModel : DoubleValueDto, IDataErrorInfo
	{
		#region Implementation of IDataErrorInfo
		public Action<bool> ValidateAction;

		public string this[string columnName]
		{
			get
			{
				var toolTip = string.Empty;
				if (Value == null)
				{
					ValidateAction(false);
					return "Field cannot be empty";
				}
				if (Math.Ceiling(Math.Log10(Value.Value)) > Parent.Configuration.Length)
				{

					ValidateAction(false);
					toolTip = "Value exceeds Max Length" + "\n" + "Max length: " + Parent.Configuration.Length;
				}

				ValidateAction(true);
				return toolTip;
			}
		}
		public string Error { get; private set; }

		#endregion
	}
}
