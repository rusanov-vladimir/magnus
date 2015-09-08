namespace Magnus.Business.Services
{
	using System;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using Domain;
	using Dtos;
	using Dtos.DynamicFields;
	using Infrastructure;
	using Interfaces;

	public class TaskService : ITaskService
	{
		private readonly IRepository _repository;
		private readonly IFileManagerService _fileManagerService;

		public TaskService(IRepository repository, IFileManagerService fileManagerService)
		{
			_repository = repository;
			_fileManagerService = fileManagerService;
		}

		public void CloseTask(Guid taskId)
		{
			var task = _repository.Query<Task>().SingleOrDefault(x => x.Id == taskId);
			if(task == null)
				return;
			task.Project.State = StateType.Finished;
			_repository.Commit();
		}

		public void SetCurrentDocumentForTask(Guid taskId, Guid? currentDocumentId)
		{
			var taskToUpdate = _repository.Query<Task>().Single(x => x.Id == taskId);
			taskToUpdate.CurrentDocumentId = currentDocumentId;
			_repository.Commit();
		}

		public void PrepareWorkspaceForTask(Guid taskId)
		{
			var task = _repository.Query<Task>().Single(x => x.Id == taskId);
			_fileManagerService.ExtractArchiveToTemp(task.Name);
		}

		public TaskDto GetOrCreateTask(string username)
		{
			var task = _repository.Query<Task>().SingleOrDefault(x =>x.CurrentUser.Username == Thread.CurrentPrincipal.Identity.Name) ??
						_repository.Query<Task>().FirstOrDefault(x =>x.CurrentUser == null);

			var user = GetUser(username);

			if (task == null)
			{
				var project = _repository.Query<Project>().FirstOrDefault(x => x.Teams.SelectMany(u => u.Users).Select(u=>u.Id).Contains(user.Id));
				if (project == null)
					return null;

				task = CreateNewTask(project,user);
			}

			List<DocumentDto> documentDtos = task.Documents.OrderBy(x=>x.Name).Select(DocumentToDto).ToList();

			return new TaskDto
			{
				Id = task.Id,
				CurrentDocument = DocumentToDto(task.CurrentDocument),
				Documents = documentDtos,
				Name = task.Name,
				Project = new ProjectDto 
				{
					Name = task.Project.Name,
					Id = task.Project.Id
				},
				CurrentUser = new UserDto
				{
					Id = user.Id,
					Username = user.Username,
					Password = user.Password
				},
			};
		}

		private Task CreateNewTask(Project project, User user)
		{
			var path = project.WorkingDirectory;
			var extractionPath = _fileManagerService.Catalog;
			_fileManagerService.Catalog = path;

			_fileManagerService.ReadFileNames(new List<string>
			{
				".zip"
			});

			var takenIntoElaboration =
				_repository.Query<Task>()
							.Where(x => x.Project.Id == project.Id && _fileManagerService.Files.Contains(x.Name))
							.Select(x => x.Name);

			var toElaborate = _fileManagerService.Files.Except(takenIntoElaboration).FirstOrDefault();
			if (toElaborate == null)
			{
				throw new Exception("No tasks to elaborate, check if there are remaining archives");
			}

			_fileManagerService.ExtractArchiveToTemp(Path.Combine(path, toElaborate), null);

			_fileManagerService.Catalog = extractionPath;
			_fileManagerService.ReadFileNames();

			var documents = _fileManagerService.Files.Select(
				file => new Document
				{
					Name = file
				}).OrderBy(x=>x.Name).ToList();
			
			var task = new Task
			{
				Documents = documents,
				CurrentUser = user,
				//CurrentDocumentId = documents.First().Id,
				Name = toElaborate,
				Project = project
			};
			_repository.Add(task);
			_repository.Commit();
			return task;
		}

		private static DocumentDto DocumentToDto(Document document)
		{
			if (document == null)
				return null;
			var documentDto = new DocumentDto
			{
				Name = document.Name,
				Id = document.Id
			};

			var fieldsCollection = new Collection<DynamicFieldDto>();
			foreach (var field in document.Fields)
			{
				var dFieldDto = new DynamicFieldDto
				{
					Configuration = new DynamicFieldTemplateDto()
					{
						Code = field.Configuration.Code,
						Id = field.Configuration.Id,
						IsEnabled = field.Configuration.IsEnabled,
						Label = field.Configuration.Label,
						Length = field.Configuration.Length,
						Type = field.Configuration.Type,
						Weight = field.Configuration.Weight,
					},
					Id = field.Id
				};
				dFieldDto.SetValue(field.GetValue());

				fieldsCollection.Add(dFieldDto);
			}

			documentDto.Fields = fieldsCollection;
			return documentDto;
		}

		public User GetUser(string username)
		{
			var user = _repository.Query<User>().SingleOrDefault(x => x.Username == username);
			return user;
		}
	}
}
