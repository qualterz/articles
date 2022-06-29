using Articles.ViewModel;
using System.Windows.Controls;

namespace Articles.View
{
    /// <summary>
    /// Interaction logic for ProfileTabControl.xaml
    /// </summary>
    public partial class DashboardTabControl : UserControl
    {
        public DashboardTabControl()
        {
            InitializeComponent();

            DataContext = new DashboardTabViewModel();
        }
    }
}
