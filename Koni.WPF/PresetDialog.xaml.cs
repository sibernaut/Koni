using System.Windows;
using System.Windows.Input;

namespace Koni.WPF
{
    /// <summary>
    /// Interaction logic for PresetDialog.xaml
    /// </summary>
    public partial class PresetDialog : Window
    {
        public PresetDialog()
        {
            InitializeComponent();
            Title = "New Preset";
        }

        public PresetDialog(string search, string replace)
        {
            InitializeComponent();
            Title = "Edit Preset";
            SearchFor.Text = search;
            ReplaceWith.Text = replace;
        }

        private void Commands_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
