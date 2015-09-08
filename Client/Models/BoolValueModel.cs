namespace Client.Models
{
	using System;
	using System.ComponentModel;
	using Magnus.Business.Dtos;
	using Magnus.Business.Dtos.DynamicFields;

	class BoolValueModel : BoolValueDto, IDataErrorInfo
	{
		public Action<bool> ValidateAction;
		#region Implementation of IDataErrorInfo

		public string this[string columnName]
		{
			get
			{
				if (Value == null)
				{
					ValidateAction(false);
					return "Field cannot be empty";
				}
				ValidateAction(true);
				return string.Empty;
			}
		}
		public string Error { get; private set; }

		#endregion
	}
}
