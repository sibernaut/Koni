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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public Config Config = new();
        public SettingsWindow()
        {
            InitializeComponent();
            Config.Load();
            PresetsList.ItemsSource = Config.Presets;
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new PresetDialog();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                Config.Add(dialog.Preset);
                Config.Save();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = PresetsList.SelectedIndex;
            var selectedItem = PresetsList.SelectedItem as Preset;
            var dialog = new PresetDialog(selectedItem);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                Config.Update(selectedIndex, dialog.Preset);
                Config.Save();
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            Config.MoveUp(PresetsList.SelectedIndex);
            Config.Save();
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            Config.MoveDown(PresetsList.SelectedIndex);
            Config.Save();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var msgBox = MessageBox.Show("Do you want to delete this preset?", 
                "Delete preset", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            var selected = PresetsList.SelectedItem as Preset;
            if (msgBox == MessageBoxResult.Yes)
            {
                Config.Remove(selected);
                Config.Save();
            }
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
