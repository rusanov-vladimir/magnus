namespace Client.ViewModels
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media.Imaging;
	using Interfaces;
	using Microsoft.Practices.Prism.Commands;
	using Microsoft.Practices.Prism.Mvvm;
	using ZXing.Common;
	using ZXing.Presentation;

	public class ImageVisualizationViewModel : BindableBase, IImageVisualizationViewModel, IDisposable
	{
		private bool _automaticSnapMode = true;
		private string _filePath;
		private BitmapSource _image;
		private Stream _imageStreamSource;
		private int _maxPages;
		private int _page;
		private Point _renderOriginPoint = new Point(0.5, 0.5);
		private double _rotationAngle;
		private double _scaleX = 1;
		private double _scaleY = 1;
		private TiffBitmapDecoder _tiffDecoder;
		public static string CurrentImage;


		public ImageVisualizationViewModel()
		{


			//Rotate Image in Clockwise Direction
			RotateClockWiseCommand = new DelegateCommand(() => { RotationAngle = RotationAngle > 270 ? 90 : RotationAngle + 90; });

			//Rotate Image in Counter Clockwise Direction
			RotateCounterClockWiseCommand =
				new DelegateCommand(() => { RotationAngle = RotationAngle < 90 ? 270 : RotationAngle - 90; });

			//Zoom In the Image
			ZoomInCommand = new DelegateCommand(() =>
												{
													if (ScaleX < ScaleY)
														ScaleX = ScaleY = 2;
													else
														ScaleX = ScaleY = ScaleY * 2;
													RaiseCanExecuteChangedForManualScroll();
												},
				() => ScaleY < 16);

			//Zoom Out the Image
			ZoomOutCommand = new DelegateCommand(() =>
												{
													if (ScaleX < ScaleY)
														ScaleY = ScaleX = 1;
													else
														ScaleX = ScaleY = ScaleY / 2;
													RaiseCanExecuteChangedForManualScroll();
												},
				() => ScaleY > 1);


			ScrollUpCommand = new DelegateCommand(() =>
												{
													MoveRenderOrigin(0, -0.05);
													RaiseCanExecuteChangedForManualScroll();
												},
				() => RenderOriginPoint.Y > 0.04 && ScaleY > 1);


			ScrollDownCommand = new DelegateCommand(() =>
													{
														MoveRenderOrigin(0, 0.05);
														RaiseCanExecuteChangedForManualScroll();
													},
				() => RenderOriginPoint.Y < 0.96 && ScaleY > 1);
			ScrollLeftCommand = new DelegateCommand(() =>
													{
														MoveRenderOrigin(-0.05, 0);
														RaiseCanExecuteChangedForManualScroll();
													},
				() => RenderOriginPoint.X > 0.04 && ScaleY > 1 && ScaleX > 1);
			ScrollRightCommand = new DelegateCommand(() =>
													{
														MoveRenderOrigin(0.05, 0);
														RaiseCanExecuteChangedForManualScroll();
													},
				() => RenderOriginPoint.X < 0.96 && ScaleY > 1 && ScaleX > 1);


			MoveViewPositionCommand = new DelegateCommand<string>(
				position =>
				{
					if (string.IsNullOrWhiteSpace(position))
						return;
					var pos = (OriginPosition) Enum.Parse(typeof(OriginPosition), position);
					MoveRenderOrigin(pos);
					RaiseCanExecuteChangedForManualScroll();
				});


			NextPageCommand = new DelegateCommand(() =>
												{
													Image = _tiffDecoder.Frames[++_page];
													PageFlipedOrImageChanged();
												},
				() => _page < _maxPages);

			PrevPageCommand = new DelegateCommand(() =>
												{
													Image = _tiffDecoder.Frames[--_page];
													PageFlipedOrImageChanged();
												},
				() => _page > 0);

			DecodeBarcodeCommand = new DelegateCommand(DecodeBarCode, () => Image != null);

			SnapModeChangedCommand = new DelegateCommand(RaiseCanExecuteChangedForManualScroll);


			MoveRenderOrigin(OriginPosition.Top);
			RaiseCanExecuteChangedForManualScroll();
		}


		public string DisplayFileName
		{
			get
			{
				CurrentImage = Path.GetFileName(FilePath);
				return CurrentImage;
			}
		}


		public void Dispose()
		{
			if(_imageStreamSource!=null)
				_imageStreamSource.Dispose();
		}

		public BitmapSource Image
		{
			get { return _image; }
			set { SetProperty(ref _image, value); }
		}

		public double ScaleY
		{
			get { return _scaleY; }
			set
			{
				SetProperty(ref _scaleY, value);
				OnPropertyChanged(() => ZoomLevel);
				OnPropertyChanged(() => XViewPort);
				OnPropertyChanged(() => YViewPort);
			}
		}

		public double ScaleX
		{
			get { return _scaleX; }
			set
			{
				SetProperty(ref _scaleX, value);
				OnPropertyChanged(() => ZoomLevel);
				OnPropertyChanged(() => XViewPort);
				OnPropertyChanged(() => YViewPort);
			}
		}

		public double ZoomLevel
		{
			get { return ScaleY*ScaleX * 100; }
		}

		public double RotationAngle
		{
			get { return _rotationAngle; }
			set { SetProperty(ref _rotationAngle, value); }
		}

		public Point RenderOriginPoint
		{
			get { return _renderOriginPoint; }
			set
			{
				SetProperty(ref _renderOriginPoint, value);
				OnPropertyChanged(()=>X);
				OnPropertyChanged(()=>Y);
			}
		}


		public double X
		{
			get { return _renderOriginPoint.X; }
			set
			{
				_renderOriginPoint = new Point(value,_renderOriginPoint.Y);
				OnPropertyChanged(()=>RenderOriginPoint);
			}
		}

		public double Y
		{
			get { return _renderOriginPoint.Y; }
			set
			{
				_renderOriginPoint = new Point(_renderOriginPoint.X, value);
				OnPropertyChanged(() => RenderOriginPoint);
			}
		}

		public string FilePath
		{
			get { return _filePath; }
			set
			{
				SetProperty(ref _filePath, value);

				//trying to clear the resources
				if (_imageStreamSource != null)
				{
					_imageStreamSource.Dispose();
					Image = null;
				}
				ReadFile();
				OnPropertyChanged(() => DisplayFileName);
			}
		}

		public bool AutomaticSnapMode
		{
			get { return _automaticSnapMode; }
			set { SetProperty(ref _automaticSnapMode, value); }
		}

		public double XViewPort
		{
			get { return 2 / ScaleX; }
		}

		public double YViewPort
		{
			get { return 2 / ScaleY; }
		}

		public DelegateCommand RotateClockWiseCommand { get; }
		public DelegateCommand RotateCounterClockWiseCommand { get; }
		public DelegateCommand ZoomInCommand { get; }
		public DelegateCommand ZoomOutCommand { get; }
		public DelegateCommand ScrollUpCommand { get; }
		public DelegateCommand ScrollDownCommand { get; }
		public DelegateCommand ScrollLeftCommand { get; }
		public DelegateCommand ScrollRightCommand { get; }
		public DelegateCommand NextPageCommand { get; }
		public DelegateCommand PrevPageCommand { get; }
		public DelegateCommand SnapModeChangedCommand { get; }
		public DelegateCommand DecodeBarcodeCommand { get; }
		public DelegateCommand<string> MoveViewPositionCommand { get; }

		private void PageFlipedOrImageChanged()
		{
			PrevPageCommand.RaiseCanExecuteChanged();
			NextPageCommand.RaiseCanExecuteChanged();
			RotationAngle = 0;
			MoveRenderOrigin(OriginPosition.Top);
		}

		private void RaiseCanExecuteChangedForManualScroll()
		{
			ScrollDownCommand.RaiseCanExecuteChanged();
			ScrollUpCommand.RaiseCanExecuteChanged();
			ScrollLeftCommand.RaiseCanExecuteChanged();
			ScrollRightCommand.RaiseCanExecuteChanged();
		}

		private void ReadFile()
		{
			// Open a Stream and decode a TIFF image
			_imageStreamSource = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			_tiffDecoder = new TiffBitmapDecoder(_imageStreamSource,
				BitmapCreateOptions.PreservePixelFormat,
				BitmapCacheOption.Default);

			_page = 0;
			Image = _tiffDecoder.Frames[0];
			_maxPages = _tiffDecoder.Frames.Count - 1;
			PageFlipedOrImageChanged();
		}

		private async void DecodeBarCode()
		{
			var reader = new BarcodeReader
			{
				AutoRotate = true,
				Options = new DecodingOptions
				{
					TryHarder = true,
				}
			};

			var result = await Task.Run(() => reader.Decode(Image));

			Debug.WriteLine(result != null
				? "Barcode text : " + result.Text + "\nFormat : " + result.BarcodeFormat
				: "Barcode not found!");
		}

		private void MoveRenderOrigin(double x, double y)
		{
			RenderOriginPoint = new Point(_renderOriginPoint.X + x,
				_renderOriginPoint.Y + y);
		}

		private void MoveRenderOrigin(OriginPosition position)
		{
			if (position == OriginPosition.Center)
			{
				RenderOriginPoint = new Point(0.5, 0.5);
				ScaleX = ScaleY = 1;
				return;
			}
			if (position == OriginPosition.Top)
			{
				RenderOriginPoint = new Point(0.5, 0);
				ScaleX = 1;
				ScaleY = 1.8;
				return;
			}

			if (position == OriginPosition.Bottom)
			{
				RenderOriginPoint = new Point(0.5, 1);
				ScaleX = 1;
				ScaleY = 1.8;
				return;
			}

			if (position == OriginPosition.TopLeft)
				RenderOriginPoint = new Point(0, 0);

			if (position == OriginPosition.TopRight)
				RenderOriginPoint = new Point(1, 0);

			if (position == OriginPosition.BottomLeft)
				RenderOriginPoint = new Point(0, 1);

			if (position == OriginPosition.BottomRight)
				RenderOriginPoint = new Point(1, 1);
			ScaleX = ScaleY = 2;
		}

		private enum OriginPosition
		{
			Center = 0,
			Top = 1,
			Bottom = 2,
			TopLeft = 3,
			TopRight = 4,
			BottomLeft = 5,
			BottomRight = 6
		}
	}
}