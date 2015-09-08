namespace Magnus.Business.Services.Interfaces
{
	using System;
	using Domain;
	using Dtos;

	public interface ITaskService
	{
		TaskDto GetOrCreateTask(string username);
		User GetUser(string username);
		void CloseTask(Guid taskId);
		void SetCurrentDocumentForTask(Guid taskId, Guid? currentDocumentId);
		void PrepareWorkspaceForTask(Guid taskId);
	}
}
