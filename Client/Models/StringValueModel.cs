namespace Client.Models
{
	using System;
	using System.ComponentModel;
	using Magnus.Business.Dtos;
	using Magnus.Business.Dtos.DynamicFields;

	public class StringValueModel : StringValueDto, IDataErrorInfo
	{
		#region Implementation of IDataErrorInfo

		public Action<bool> ValidateAction;

		public string this[string columnName]
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Value))
				{
					ValidateAction(false);
					return "Field cannot be empty";
				}

				if (Value.Length > Parent.Configuration.Length)
				{
					ValidateAction(false);
					return "Maximum length is " + Parent.Configuration.Length;
				}

				ValidateAction(true);
				return string.Empty;
			}
		}
		public string Error { get; private set; }

		#endregion
	}
}
