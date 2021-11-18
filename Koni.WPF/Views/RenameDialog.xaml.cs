// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

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
