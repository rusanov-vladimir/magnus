namespace Client.ViewModels
{
	using System.Threading.Tasks;
	using Interfaces;
	using Magnus.Business.Services.Interfaces;
	using Microsoft.Practices.Prism.Commands;
	using Microsoft.Practices.Prism.Mvvm;
	using System;
	using System.ComponentModel;
	using System.Threading;
	using System.Windows;
	using System.Windows.Input;
	using Magnus.Business.Dtos;
	using Magnus.Business.Infrastructure;
	using Properties;
	using Views;

	public class LoginViewModel : BindableBase, ILoginViewModel, IDataErrorInfo
	{
		private int _authenticationType;
		private string _error;
		private bool _isAdornerVisible;
		private string _password;
		private bool _rememberUser;
		private string _username;
		private string _validation;
		private readonly ILoginService _loginService;

		public LoginViewModel(ILoginService loginService)
		{
			_loginService = loginService;
			LoginCommand = new DelegateCommand( async () => await TryLogin());

			if (!(bool)  Settings.Default["IsAutoLogin"])
				return;
				_authenticationType = 1;
				RememberUser = true;
				Username = Settings.Default["Username"].ToString();
				Password = Settings.Default["Password"].ToString();
			}

		private async Task TryLogin()
		{
			IsAdornerVisible = true;
			UserDto user = null;
			if (AuthenticationType == 1)
			{
				user = await Task.Run(() => _loginService.LogIn(Username, Password));

				if (user == null)
				{
					Validation = "Wrong username or password!";
					Password = string.Empty;
					IsAdornerVisible = false;
					return;
				}

				if (RememberUser)
					SaveSettings();
			}

			var appPrincipal = (AppPrincipal)Thread.CurrentPrincipal;
			appPrincipal.Identity = new AppIdentity(user.Id, user.Username, user.Project.Id, string.Empty, null);

			var mainWindow = new MainWindow();
			Application.Current.MainWindow.Close();
			Application.Current.MainWindow = mainWindow;
			mainWindow.Show();
		}

		private void SaveSettings()
		{
			Settings.Default["IsAutoLogin"] = true;
			Settings.Default["Username"] = Username;
			Settings.Default["Password"] = Password;
			Settings.Default.Save();
		}

		#region Implementation of ILoginViewModel

		public string Username
		{
			get { return _username; }
			set { SetProperty(ref _username, value); }
		}
		public string Password
		{
			get { return _password; }
			set { SetProperty(ref _password, value); }
		}

		public bool RememberUser
		{
			get { return _rememberUser; }
			set { SetProperty(ref _rememberUser, value); }
		}

		public int AuthenticationType
		{
			get { return _authenticationType; }
			set { SetProperty(ref _authenticationType, value); }
		}
		public bool IsAdornerVisible
		{
			get { return _isAdornerVisible; }
			set { SetProperty(ref _isAdornerVisible, value); }
		}
		public string Validation
		{
			get { return _validation; }
			set { SetProperty(ref _validation, value); }
		}


		public ICommand LoginCommand { get; private set; }

		#endregion

		#region Implementation of IDataErrorInfo

		public string this[string columnName]
		{
			get
			{
				//TODO [VR,20150730] review this
				var result = string.Empty;

				if (AuthenticationType == 0)
					return result;

				
				if (columnName == "Username")
				{
					if (Username.Length > 16)
						result = "Username must be shorter than 16 characters.";
					if (String.IsNullOrWhiteSpace(Username))
						result = "Username cannot be empty";
				}
				else if (columnName == "Password")
				{
					if (Password.Length > 16)
						result = "Password must be shorter than 16 characters.";
					if (String.IsNullOrWhiteSpace(Password))
						result = "Password cannot be empty";
				}

				return result;
			}
		}
		public string Error
		{
			get { return _error; }
		}

		#endregion
	}

}