namespace Client.Interfaces
{
	using System;
	using Magnus.Business.Dtos;
	using Microsoft.Practices.Prism.Commands;

	public interface IFieldsViewModel : IDisposable
	{
		Guid TaskId { get; set; }
		int TotalFieldsNumber { get; set; }
		string ValidationResult { get; set; }
		string Page { get; }
		bool IsSubmitActive { get; }
		DelegateCommand PrevPageCommand { get; }
		DelegateCommand NextPageCommand { get; }
		DelegateCommand ExportStaticCommand { get; }
		DelegateCommand ExportDynamicCommand { get; }
		void OnNavigationHappened(TaskDto taskDto, bool forward);
	}
}