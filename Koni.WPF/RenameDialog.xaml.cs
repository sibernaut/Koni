// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Koni.WPF
{
    /// <summary>
    /// Interaction logic for RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog : Window
    {
        public string RenamedTitle;

        public RenameDialog(string filename, string title)
        {
            InitializeComponent();
            FilenameLabel.Content = filename;
            TitleTextBox.Text = title;
            TitleTextBox.Focus();
            TitleTextBox.SelectAll();
        }

        private void Commands_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = true;
            RenamedTitle = TitleTextBox.Text;
            Close();
        }

        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
