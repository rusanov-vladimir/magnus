namespace Client.ViewModels
{
	using System;
	using System.Linq;
	using Interfaces;
	using Magnus.Business.Services.Interfaces;
	using Microsoft.Practices.Prism.Commands;
	using Microsoft.Practices.Prism.Mvvm;

	public class FooterNavigationViewModel : BindableBase, IFooterNavigationViewModel
	{
		private readonly IFileManagerService _fileManagerService;
		private int _currentFileNumber;
		private int _filesCount;

	
		public FooterNavigationViewModel(IFileManagerService fileManagerService)
		{
			_fileManagerService = fileManagerService;
			
			NextFileCommand = new DelegateCommand(
				GetNextFile,
				() => _currentFileNumber < _fileManagerService.FilesCount);

			PreviousFileCommand = new DelegateCommand(
				GetPreviousFile,
				() => _currentFileNumber > 1);

			GoToFirstFile = new DelegateCommand(GetFirstFile);

			
		}

		public event Action<string, bool> NavigationHappened;
		public string LoadDocumentAndReturnItsPath(string name)
		{
			_fileManagerService.ReadFileNames();
			FilesCount = _fileManagerService.FilesCount;
			var file = _fileManagerService.Files.Single(x => x.Contains(name));
			CurrentFileNumber = _fileManagerService.Files.IndexOf(file)+1;
			_fileManagerService.CurrentFileNumber = _fileManagerService.Files.IndexOf(file) + 1;
			RaiseCanExecuteChangedForFooterCommands();
			return file;
		}

		public DelegateCommand NextFileCommand { get; private set; }
		public DelegateCommand PreviousFileCommand { get; private set; }
		public DelegateCommand GoToFirstFile { get; private set; }

		public int FilesCount
		{
			get { return _filesCount; }
			private set { SetProperty(ref _filesCount, value); }
		}
		public int CurrentFileNumber
		{
			get { return _currentFileNumber; }
			private set { SetProperty(ref _currentFileNumber, value); }
		}

		private void GetFirstFile()
		{
			_fileManagerService.ReadFileNames();
			CurrentFileNumber = 1;
			FilesCount = _fileManagerService.FilesCount;
			OnNavigationHappened(_fileManagerService.Files[0],true);
		}

		private void GetPreviousFile()
		{
			_fileManagerService.GetPreviousFile();
			MoveToNextImage(false);
		}

		private void GetNextFile()
		{
			_fileManagerService.GetNextFile();
			MoveToNextImage(true);
		}

		protected virtual void OnNavigationHappened(string filePath, bool forward)
		{
			var handler = NavigationHappened;
			if (handler != null)
				handler(filePath, forward);
		}

		private void MoveToNextImage(bool forward)
		{
			if (_fileManagerService.CurrentFile == null)
				return;
			CurrentFileNumber = _fileManagerService.CurrentFileNumber;
			FilesCount = _fileManagerService.FilesCount;
			OnNavigationHappened(_fileManagerService.CurrentFile, forward);
			RaiseCanExecuteChangedForFooterCommands();
		}

		private void RaiseCanExecuteChangedForFooterCommands()
		{
			NextFileCommand.RaiseCanExecuteChanged();
			PreviousFileCommand.RaiseCanExecuteChanged();
			GoToFirstFile.RaiseCanExecuteChanged();
		}
	}
}