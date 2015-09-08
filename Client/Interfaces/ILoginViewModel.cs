namespace Client.Interfaces
{
	using System.Windows.Input;

	public interface ILoginViewModel
	{
		string Username { get; set; }
		string Password { get; set; }
		bool RememberUser { get; set; }
		int AuthenticationType { get; set; }
		bool IsAdornerVisible { get; set; }
		string Validation { get; set; }
		ICommand LoginCommand { get; }
	}
}
