namespace Client.Interfaces
{
	using System;
	using System.Windows;
	using System.Windows.Media.Imaging;
	using Microsoft.Practices.Prism.Commands;

	public interface IImageVisualizationViewModel : IDisposable
	{
		BitmapSource Image { get; set; }
		double ScaleY { get; set; }
		double ScaleX { get; set; }
		double ZoomLevel { get; }
		double RotationAngle { get; set; }
		Point RenderOriginPoint { get; set; }
		string FilePath { get; set; }
		bool AutomaticSnapMode { get; set; }
		double XViewPort { get; }
		double YViewPort { get; }
		DelegateCommand RotateClockWiseCommand { get; }
		DelegateCommand RotateCounterClockWiseCommand { get; }
		DelegateCommand ZoomInCommand { get; }
		DelegateCommand ZoomOutCommand { get; }
		DelegateCommand ScrollUpCommand { get; }
		DelegateCommand ScrollDownCommand { get; }
		DelegateCommand ScrollLeftCommand { get; }
		DelegateCommand ScrollRightCommand { get; }
		DelegateCommand NextPageCommand { get; }
		DelegateCommand PrevPageCommand { get; }
		DelegateCommand SnapModeChangedCommand { get; }
		DelegateCommand DecodeBarcodeCommand { get; }
		DelegateCommand<string> MoveViewPositionCommand { get; }
	}
}