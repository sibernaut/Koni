// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Koni.Engine.Wrapper;
using System.IO.Abstractions;
using System.Windows;
using System.Windows.Input;

namespace Koni.WPF
{
    /// <summary>
    /// Interaction logic for PresetsWindow.xaml
    /// </summary>
    public partial class PresetsWindow : Window
    {
        Config config = new Config(new FileSystem());
        Presets presets;
        public PresetsWindow()
        {
            InitializeComponent();
            config.Load();
            presets = config.Presets;
            PresetsListBox.ItemsSource = presets.Items;
        }

        private void Commands_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = true;
            config.Save();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new PresetDialog();
            if (dialog.ShowDialog() == true)
            {
                var search = dialog.SearchFor.Text;
                var replace = dialog.ReplaceWith.Text;
                presets.Add(search, replace);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (PresetsListBox.SelectedIndex != -1)
            {
                var index = PresetsListBox.SelectedIndex;
                var item = PresetsListBox.SelectedItem as Preset;
                var dialog = new PresetDialog(item.SearchFor, item.ReplaceWith);
                if (dialog.ShowDialog() == true)
                {
                    var search = dialog.SearchFor.Text;
                    var replace = dialog.ReplaceWith.Text;
                    presets.Update(index, search, replace);
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PresetsListBox.SelectedIndex != -1)
                presets.Delete(PresetsListBox.SelectedIndex);
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (PresetsListBox.SelectedIndex != -1 || PresetsListBox.SelectedIndex != 0)
                presets.MoveUp(PresetsListBox.SelectedIndex);
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (PresetsListBox.SelectedIndex != -1 || PresetsListBox.SelectedIndex < PresetsListBox.Items.Count)
                presets.MoveDown(PresetsListBox.SelectedIndex);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
