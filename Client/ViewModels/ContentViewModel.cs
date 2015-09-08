namespace Client.ViewModels
{
	using Interfaces;
	using Microsoft.Practices.Prism.Mvvm;

	public class ContentViewModel : BindableBase, IContentViewModel
	{
		public ContentViewModel(IImageVisualizationViewModel imageVisualizationViewModel, IFieldsViewModel fieldsViewModel)
		{
			Fields = fieldsViewModel;
			ImageVisualization = imageVisualizationViewModel;
		}

		public IFieldsViewModel Fields { get; set; }
		public IImageVisualizationViewModel ImageVisualization { get; set; }
		public void Dispose()
		{
			ImageVisualization.Dispose();
			Fields.Dispose();
		}
	}
}