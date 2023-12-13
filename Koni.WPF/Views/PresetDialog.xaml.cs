using Koni.WPF.ViewModels;
using System.Windows;

namespace Koni.WPF.Views
{
    /// <summary>
    /// Interaction logic for PresetDialog.xaml
    /// </summary>
    public partial class PresetDialog : Window
    {
        public PresetViewModel Preset;

        public PresetDialog()
        {
            InitializeComponent();
            Title = "New preset";
            SaveButton.IsDefault = true;
            SearchTextBox.Focus();
        }

        public PresetDialog(PresetViewModel preset)
        {
            InitializeComponent();
            Title = "Edit preset";
            CancelButton.IsDefault = true;
            SearchTextBox.Text = preset.SearchFor;
            SearchTextBox.SelectAll();
            SearchTextBox.Focus();
            ReplaceTextBox.Text = preset.ReplaceWith;
            ReplaceTextBox.SelectAll();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Preset = new(SearchTextBox.Text, ReplaceTextBox.Text);
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
