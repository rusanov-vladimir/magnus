namespace Client.ViewModels
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Windows;
	using Interfaces;
	using Magnus.Business.Dtos;
	using Magnus.Business.Services.Interfaces;
	using Microsoft.Practices.Prism.Commands;
	using Microsoft.Practices.Prism.Mvvm;
	using Properties;
	using Views;

	public class MainWindowViewModel : BindableBase
	{
		private readonly ITaskService _taskService;

		public MainWindowViewModel(IFooterNavigationViewModel footer, IContentViewModel content, ITaskService taskService)
		{
			_taskService = taskService;
			LogOutCommand = new DelegateCommand(() =>
			{
				LogOut(content);
			});

			var task = _taskService.GetOrCreateTask(Thread.CurrentPrincipal.Identity.Name);
			_taskService.PrepareWorkspaceForTask(task.Id);
			
			if(task == null)
				throw new ArgumentException("Task cannot be null");

			Footer = footer;
			Content = content;
			footer.NavigationHappened += OnFooterNavigation;
			
			

			if (task.CurrentDocument == null){
				footer.GoToFirstFile.Execute().Wait();
			}
			else
			{
				var path = Footer.LoadDocumentAndReturnItsPath(task.CurrentDocument.Name);
				Content.ImageVisualization.FilePath = path;
				Content.Fields.TotalFieldsNumber = task.Documents.Count;
				Content.Fields.TaskId = task.Id;
				content.Fields.OnNavigationHappened(task, true);
			}
		}

		private static void LogOut(IContentViewModel content)
		{
			Thread.CurrentPrincipal = null;

			Settings.Default["IsAutoLogin"] = false;
			Settings.Default.Save();
			var view = new Login();
			Application.Current.MainWindow.Close();
			Application.Current.MainWindow = view;
			view.Show();
			content.Dispose();
		}

		public IFooterNavigationViewModel Footer { get; set; }
		public IContentViewModel Content { get; set; }

		private void OnFooterNavigation(string filePath, bool forward)
		{
			var task = _taskService.GetOrCreateTask(Thread.CurrentPrincipal.Identity.Name);

			
			if (task.CurrentDocument == null)
			{ 
				task.CurrentDocument = task.Documents.First();
			}
			else
			{
				
				int index = task.Documents.IndexOf(task.Documents.FirstOrDefault(x => x.Id == task.CurrentDocument.Id));
				DocumentDto itemPrev = null;
				DocumentDto itemNext = null;
				if (index > 0)
					itemPrev = task.Documents.ElementAt(index - 1);

				if (index < task.Documents.Count-1)
					itemNext = task.Documents.ElementAt(index + 1);
				task.CurrentDocument = forward ? itemNext : itemPrev;
				
			}

			_taskService.SetCurrentDocumentForTask(task.Id, task.CurrentDocument == null ? (Guid?) null : task.CurrentDocument.Id);
			Content.ImageVisualization.FilePath = filePath;
			Content.Fields.TotalFieldsNumber = task.Documents.Count;
			Content.Fields.TaskId = task.Id;

			Content.Fields.OnNavigationHappened(task, forward);
		}

		public DelegateCommand LogOutCommand { get; private set; }

	}
}