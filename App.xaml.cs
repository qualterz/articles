using Articles.ViewModel;
using ControlzEx.Theming;
using System;
using System.IO;
using System.Windows;

namespace Articles
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string configPath { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            string configDirectoryPath = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData
                ),
                "Articles"
            );

            if (!Directory.Exists(configDirectoryPath))
                Directory.CreateDirectory(configDirectoryPath);

            configPath = Path.Combine(configDirectoryPath, "config.json");
            Config.Instance.Load(configPath);

            var databaseDirectory = Path.Combine(configDirectoryPath, "data");

            if (!Directory.Exists(databaseDirectory))
                Directory.CreateDirectory(databaseDirectory);

            if (string.IsNullOrEmpty(Config.Instance.DatabasePath))
                Config.Instance.DatabasePath
                    = Path.Combine(databaseDirectory, "current.db");

            LoginViewModel.Instance.Username = Config.Instance.Username;
            LoginViewModel.Instance.LoginCommand.Execute(null);

            if (string.IsNullOrEmpty(Config.Instance.Theme))
                Config.Instance.Theme = "Light";

            if (string.IsNullOrEmpty(Config.Instance.Color))
                Config.Instance.Color = "Blue";

            var theme = Config.Instance.Theme;
            var color = Config.Instance.Color;

            ThemeManager.Current.ChangeTheme(this, theme, color);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Config.Instance.Username = LoginViewModel.Instance.User?.Name;

            Config.Instance.Save(configPath);
        }
    }
}
