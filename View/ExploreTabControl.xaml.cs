using Articles.ViewModel;
using System.Windows.Controls;

namespace Articles.View
{
    /// <summary>
    /// Interaction logic for ExploreTabControl.xaml
    /// </summary>
    public partial class ExploreTabControl : UserControl
    {
        public ExploreTabControl()
        {
            InitializeComponent();

            DataContext = new ExploreTabViewModel();
        }
    }
}
