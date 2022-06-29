using Articles.Infrastructure;
using Articles.View;
using ControlzEx.Theming;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Articles.ViewModel
{
    public class MainViewModel : BaseNotifyPropertyChanged
    {
        public ICommand ThemeDarkCommand { get; private set; }
        public ICommand ThemeLightCommand { get; private set; }
        public ICommand ColorRedCommand { get; private set; }
        public ICommand ColorGreenCommand { get; private set; }
        public ICommand ColorBlueCommand { get; private set; }

        public ICommand LogoutCommand { get; private set; }
            = LoginViewModel.Instance.LogoutCommand;

        private ContentControl dashboardTab;
        public ContentControl DashboardTab
        {
            get => dashboardTab;
            private set
            {
                dashboardTab = value;
                Notify();
            }
        }

        private ContentControl exploreTab;
        public ContentControl ExploreTab
        {
            get => exploreTab;
            private set
            {
                exploreTab = value;
                Notify();
            }
        }

        public MainViewModel()
        {
            InitializeCommands();

            LoginViewModel.Instance.UserChanged += LoginCheck;
            LoginViewModel.Instance.UserChanged += ReloadTabs;

            LoginCheck(this, new EventArgs());
            ReloadTabs(this, new EventArgs());
        }

        private void InitializeCommands()
        {
            ThemeDarkCommand = new RelayCommand(_ =>
            {
                Config.Instance.Theme = "Dark";
                ReloadTheme();
            });

            ThemeLightCommand = new RelayCommand(_ =>
            {
                Config.Instance.Theme = "Light";
                ReloadTheme();
            });

            ColorRedCommand = new RelayCommand(_ =>
            {
                Config.Instance.Color = "Red";
                ReloadTheme();
            });

            ColorGreenCommand = new RelayCommand(_ =>
            {
                Config.Instance.Color = "Green";
                ReloadTheme();
            });

            ColorBlueCommand = new RelayCommand(_ =>
            {
                Config.Instance.Color = "Blue";
                ReloadTheme();
            });
        }

        private void ReloadTheme()
        {
            var theme = Config.Instance.Theme;
            var color = Config.Instance.Color;

            ThemeManager.Current.ChangeTheme(App.Current, theme, color);
        }

        private void ReloadTabs(object sender, EventArgs e)
        {
            DashboardTab = new DashboardTabControl();
            ExploreTab = new ExploreTabControl();
        }

        private void LoginCheck(object sender, EventArgs e)
        {
            if (!LoginViewModel.Instance.IsLogged)
                LoginViewModel.Authorization();
        }
    }
}
