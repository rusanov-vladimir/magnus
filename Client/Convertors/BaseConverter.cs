namespace Client.Convertors
{
	using System;
	using System.Globalization;
	using System.Windows.Data;
	using System.Windows.Markup;

	public abstract class BaseConverter<T> : MarkupExtension, IValueConverter
		where T : class, new()
	{
		/// <summary>
		///     Must be implemented in inheritor.
		/// </summary>
		public abstract object Convert(object value,
			Type targetType,
			object parameter,
			CultureInfo culture);

		/// <summary>
		///     Override if needed.
		/// </summary>
		public virtual object ConvertBack(object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}