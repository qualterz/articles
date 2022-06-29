using System.Windows.Controls;

namespace Articles.View
{
    /// <summary>
    /// Interaction logic for PreviewControl.xaml
    /// </summary>
    public partial class PreviewControl : UserControl
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public PreviewControl()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
