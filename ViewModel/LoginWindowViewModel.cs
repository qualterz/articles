using Articles.Infrastructure;
using System.Windows;
using System.Windows.Input;

namespace Articles.ViewModel
{
    class LoginWindowViewModel
    {
        public Window Window { get; set; }
        public string Username { get; set; }
        public ICommand SubmitCommand { get; private set; }

        public LoginWindowViewModel(Window window)
        {
            InitializeCommands();

            Window = window;
        }

        private void InitializeCommands()
        {
            SubmitCommand = new RelayCommand(_ =>
            {
                LoginViewModel.Instance.Username = Username;
                LoginViewModel.Instance.LoginCommand.Execute(null);
                if (LoginViewModel.Instance.IsLogged) Window.Close();
            });
        }
    }
}
