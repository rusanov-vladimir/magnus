namespace Client.Interfaces
{
	using System;
	using Microsoft.Practices.Prism.Commands;

	public interface IFooterNavigationViewModel
	{
		DelegateCommand NextFileCommand { get; }
		DelegateCommand PreviousFileCommand { get; }
		DelegateCommand GoToFirstFile { get; }
		int FilesCount { get; }
		int CurrentFileNumber { get; }
		event Action<string, bool> NavigationHappened;
		string LoadDocumentAndReturnItsPath(string name);
	}
}