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
using Koni.Engine;

namespace Koni.WPF
{
    /// <summary>
    /// Interaction logic for PresetDialog.xaml
    /// </summary>
    public partial class PresetDialog : Window
    {
        public Preset Preset;

        public PresetDialog()
        {
            InitializeComponent();
            Title = "New preset";
            SaveButton.IsDefault = true;
            SearchTextBox.Focus();
        }

        public PresetDialog(Preset preset)
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
