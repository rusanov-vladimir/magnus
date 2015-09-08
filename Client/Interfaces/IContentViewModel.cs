namespace Client.Interfaces
{
	using System;

	public interface IContentViewModel : IDisposable
	{
		IFieldsViewModel Fields { get; set; }
		IImageVisualizationViewModel ImageVisualization { get; set; }
	}
}