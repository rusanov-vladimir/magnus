namespace Client.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Windows.Input;
	using Interfaces;
	using Magnus.Business.Domain.DynamicFields;
	using Magnus.Business.Dtos;
	using Magnus.Business.Dtos.DynamicFields;
	using Magnus.Business.Services.Interfaces;
	using Microsoft.Practices.Prism.Commands;
	using Microsoft.Practices.Prism.Mvvm;
	using Models;
	using ThreadingTask = System.Threading.Tasks.Task;

	public class FieldsViewModel : BindableBase, IFieldsViewModel
	{
		private const int EntriesPerPage = 8;
		private int _currentFile;
		private IEnumerable<DynamicFieldTemplateDto> _dynamicFieldTemplateDtos;
		private bool _isSubmitActive;
		private int _page;
		private List<DynamicFieldModel> _tempFields = new List<DynamicFieldModel>();
		private string _validationResult;
		private readonly IDynamicFieldService _dynamicFieldService;
		private readonly IFileManagerService _fileManagerService;

		private readonly ITaskService _taskService;
		private string _projectName;
		private TaskDto _task;

		public FieldsViewModel(IDynamicFieldService dynamicFieldService,
			IFileManagerService fileManagerService,
			ITaskService taskService)
		{
			Fields = new ObservableCollection<DynamicFieldModel>();
			_dynamicFieldService = dynamicFieldService;
			_fileManagerService = fileManagerService;
			_taskService = taskService;

			_dynamicFieldTemplateDtos = new List<DynamicFieldTemplateDto>();

			SubmitData = new DelegateCommand(Submit);
			PrevPageCommand = new DelegateCommand(() => SwitchPage(--_page), () => _page > 0);
			NextPageCommand = new DelegateCommand(() => SwitchPage(++_page),
				() => _page < _dynamicFieldTemplateDtos.Count() / EntriesPerPage);

			ExportDynamicCommand =
				new DelegateCommand(() => _fileManagerService.ExportFields(false));
			ExportStaticCommand =
				new DelegateCommand(() => _fileManagerService.ExportFields(true));
		}

		public ObservableCollection<DynamicFieldModel> Fields { get; set; }
		public ICommand SubmitData { get; set; }
		public string ProjectName
		{
			get { return _projectName; }
			set { SetProperty(ref _projectName, value); }
		}
		public string ValidationResult
		{
			get { return _validationResult; }
			set { SetProperty(ref _validationResult, value); }
		}
		public string Page
		{
			get
			{
				return "Page : " + (_page + 1) + " / " +
						(_tempFields.Count() / EntriesPerPage + (_tempFields.Count() % EntriesPerPage == 0 ? 0 : 1));
			}
		}
		public DelegateCommand PrevPageCommand { get; private set; }
		public DelegateCommand NextPageCommand { get; private set; }
		public DelegateCommand ExportStaticCommand { get; private set; }
		public DelegateCommand ExportDynamicCommand { get; private set; }
		public Guid TaskId { get; set; }
		public int TotalFieldsNumber { get; set; }
		public bool IsSubmitActive
		{
			get { return _isSubmitActive; }
			set { SetProperty(ref _isSubmitActive, value); }
		}

		public void OnNavigationHappened(TaskDto taskDto, bool forward)
		{
			_task = taskDto;
			ProjectName = taskDto.Project.Name;
			_dynamicFieldTemplateDtos = _dynamicFieldService.GetDynamicFieldConfiguration(taskDto.Project.Id);

			_tempFields.Clear();
			ReCreateFields(_dynamicFieldTemplateDtos, taskDto.CurrentDocument.Fields);
			if (forward)
			{
					_currentFile++;
			}
			else
			{
				_page = 0;
			}
			
			SwitchPage(0);
		}

		public void Dispose()
		{
			//_inMemoryData.Clear();
			_dynamicFieldService.Dispose();
		}

		private void ReCreateFields(IEnumerable<DynamicFieldTemplateDto> dynamicFieldTemplateDtos,
			IEnumerable<DynamicFieldDto> existingDynamicFieldDtos)
		{
			var existingDynamicFields = existingDynamicFieldDtos ?? new List<DynamicFieldDto>();
			foreach (var dynamicFieldTemplateDto in dynamicFieldTemplateDtos)
			{
				var field = new DynamicFieldModel
				{
					Configuration = dynamicFieldTemplateDto
				};
				
				field.CreateValue(value =>
								{
									field.IsValid = value;
									IsSubmitActive = _tempFields.Count(x => x.IsValid) == _tempFields.Count;
								});


				var existingValue = existingDynamicFields.SingleOrDefault(x => x.Configuration.Code == dynamicFieldTemplateDto.Code);
				if (existingValue!=null)
				{
					field.SetValue(existingValue.GetValue());
					field.Id = existingValue.Id;
				}


				_tempFields.Add(field);
			}
		}

		private void SwitchPage(int offset)
		{
			Fields.Clear();
			for (int i = offset * EntriesPerPage;
				(offset == _tempFields.Count / EntriesPerPage)
					? (i < offset * EntriesPerPage + _tempFields.Count % EntriesPerPage)
					: (i < (offset + 1) * EntriesPerPage);
				i++)
				Fields.Add(_tempFields[i]);

			PrevPageCommand.RaiseCanExecuteChanged();
			NextPageCommand.RaiseCanExecuteChanged();
			OnPropertyChanged(() => Page);
		}

		private void Submit()
		{
			if (Validate(_tempFields) != string.Empty)
				return;

			var updatedFields = _dynamicFieldService.SaveFields(_tempFields, _task).ToList();
			if (updatedFields == null)
				throw new ArgumentNullException("updatedFields");

			ValidationResult = "Fields saved to Database successfully";

			Fields.Clear();
			for (int i = 0; i < updatedFields.Count; i++)
			{
				//TODO [VR,20150730] why do I need this???
				_tempFields[i].Id = updatedFields[i].Id;
				_tempFields[i].SetValue(updatedFields[i].GetValue());
				Fields.Add(_tempFields[i]);
			}
			

			/*if (_inMemoryData.Count(x => x.Count > 0) == TotalFieldsNumber)
				_taskService.CloseTask(TaskId);*/
			/*if (_currentFile != TotalFieldsNumber - 1)
				MainWindowViewModel.OnSwitchToNext();*/
		}

		private string Validate(IEnumerable<DynamicFieldModel> fields)
		{
			ValidationResult = string.Empty;

			foreach (var dynamicFieldDto in fields)
			{
				if (dynamicFieldDto.GetValue() != null)
				{
					if (dynamicFieldDto.Configuration.Type == DynamicFieldType.DateTime)
						continue;

					dynamicFieldDto.SetValue(dynamicFieldDto.GetValue());
					var value = dynamicFieldDto.GetValue().ToString();
					if (String.IsNullOrWhiteSpace(value) || value.Length > dynamicFieldDto.Configuration.Length)
						ValidationResult += "Field " + dynamicFieldDto.Configuration.Label + " is empty or exceeds maximum length " + "\n";
				}
				else
					ValidationResult = "Not all fields are filled." + "\n";
			}
			return ValidationResult;
		}
	}
}