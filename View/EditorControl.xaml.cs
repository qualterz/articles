using System.Windows.Controls;
using System.Windows.Input;

namespace Articles.View
{
    /// <summary>
    /// Interaction logic for EditorControl.xaml
    /// </summary>
    public partial class EditorControl : UserControl
    {
        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public EditorControl()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
