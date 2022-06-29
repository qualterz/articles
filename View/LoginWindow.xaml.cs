using Articles.ViewModel;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace Articles.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();

            DataContext = new LoginWindowViewModel(Window.GetWindow(this));
        }

        protected override void OnClosed(EventArgs e)
        {
            if (LoginViewModel.Instance.User == null)
                App.Current.Shutdown();
        }
    }
}
