namespace Magnus.Business.Services
{
	using System;
	using System.Linq;
	using Domain;
	using Dtos;
	using Infrastructure;
	using Interfaces;

	public class LoginService : ILoginService
	{
		private readonly IRepository _repository;

		public LoginService(IRepository repository)
		{
			_repository = repository;
		}

		public UserDto LogIn(UserDto userDto)
		{
			var user = _repository.Query<User>()
								.Where(x => x.Username == userDto.Login)
								.Select(x => new UserDto
								{
									Id = x.Id,
									Username = x.Username
								}).SingleOrDefault();
			return user;
		}

		public UserDto LogIn(string userName, string password)
		{
			var user = _repository.Query<User>().Where(x => x.Username == userName).
									Select(x => new UserDto()
									{
										Id = x.Id,
										Username = x.Username,
										IsAdvancedUser = x.IsAdvancedUser,
										Project = new ProjectDto()
										{
											Id = x.Team.Project.Id,
											Name = x.Team.Project.Name,
											State = x.Team.Project.State,
											WorkingDirectory = x.Team.Project.WorkingDirectory,
											//FieldTemplates = x.Team.Project.FieldTemplates
										}

										
									}).SingleOrDefault();
								
			return user;
		}
	}
}