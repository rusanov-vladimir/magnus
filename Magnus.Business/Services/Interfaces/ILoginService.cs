namespace Magnus.Business.Services.Interfaces
{
	using Dtos;

	public interface ILoginService
	{
		UserDto LogIn(UserDto user);
		UserDto LogIn(string userName, string password);
	}
}