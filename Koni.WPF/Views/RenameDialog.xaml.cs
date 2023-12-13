using Koni.WPF.ViewModels;
using System.Windows;

namespace Koni.WPF.Views
{
    /// <summary>
    /// Interaction logic for RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog : Window
    {
        public string RenamedTitle;

        public RenameDialog(VideoViewModel video)
        {
            InitializeComponent();
            FilenameLabel.Text = video.FileName;
            TitleTextBox.Text = video.Title;
            TitleTextBox.SelectAll();
            TitleTextBox.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            RenamedTitle = TitleTextBox.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
