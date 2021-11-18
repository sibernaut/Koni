// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

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
