// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Koni.Engine.Wrapper;
using System.Windows;
using System.Windows.Input;

namespace Koni.WPF
{
    /// <summary>
    /// Interaction logic for RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog : Window
    {
        private VideoFile VideoFile;
        public string RenamedTitle;

        public RenameDialog(VideoFile item)
        {
            InitializeComponent();
            VideoFile = item;
            FilenameLabel.Content = VideoFile.FileName;
            TitleTextBox.Text = VideoFile.Title;
            TitleTextBox.Focus();
            TitleTextBox.SelectAll();
        }

        public static RoutedUICommand ResetCommand = new("Reset", "Reset", typeof(RenameDialog));

        private void Commands_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ResetCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            VideoFile.Reset();
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            RenamedTitle = TitleTextBox.Text;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
