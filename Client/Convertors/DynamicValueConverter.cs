namespace Client.Convertors
{
	using System;
	using System.Globalization;

	public class DynamicValueConverter : BaseConverter<DynamicValueConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DBNull)
				return null;
			return value;
		}
	}
}