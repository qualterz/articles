using Articles.Infrastructure;
using Articles.Model;
using Articles.View;
using System;
using System.Windows.Input;

namespace Articles.ViewModel
{
    class LoginViewModel : BaseNotifyPropertyChanged
    {
        private string username;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                Notify();
            }
        }

        private User user;
        public User User
        {
            get => user;
            set
            {
                user = value;
                Notify();
            }
        }

        public event EventHandler UserChanged;

        public bool IsLogged { get => User != null; }

        public ICommand LoginCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public static readonly LoginViewModel Instance = new LoginViewModel();

        public LoginViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            LoginCommand = new RelayCommand(_ =>
            {
                if (Username == null)
                    return;

                if (Username.Length == 0)
                    return;

                using (var db = Database.Get())
                {
                    var users = db.GetCollection<User>();

                    if (!users.Exists(u => u.Name == Username))
                        users.Insert(new User(Username));

                    User = users.FindOne(u => u.Name == Username);

                    Config.Instance.Username = User.Name;
                }

                UserChanged?.Invoke(this, new EventArgs());
            });

            LogoutCommand = new RelayCommand(_ =>
            {
                User = null;

                UserChanged?.Invoke(this, new EventArgs());
            });
        }

        public static void Authorization()
        {
            var window = new LoginWindow();

            window.WindowStartupLocation =
                System.Windows.WindowStartupLocation.CenterScreen;

            window.ShowDialog();
        }
    }
}
