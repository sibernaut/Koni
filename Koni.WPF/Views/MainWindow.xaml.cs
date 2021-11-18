// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Koni.WPF.ViewModels;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Koni.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SettingsViewModel settings;
        MainViewModel queue;
        public MainWindow()
        {
            InitializeComponent();
            queue = new();
            settings = queue.Settings;
            QueueView.ItemsSource = queue.Items;
            PresetsList.ItemsSource = settings.Presets;
        }

        private void AvailableCommands_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SelectedOneCommands_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (QueueView.SelectedItem != null && QueueView.SelectedItems.Count == 1)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void SelectedItemCommands_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (QueueView.SelectedItem != null)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var dropItems = e.Data.GetData(DataFormats.FileDrop) as string[];
                queue.AddRange(dropItems);
            }
        }

        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter =
                    "Video files (*.mp4;*.mkv)|*.mp4;*.mkv|MP4 files (*.mp4)|*.mp4|Matroska files (*.mkv)|*.mkv",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
                queue.AddRange(openFileDialog.FileNames);
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = QueueView.SelectedItems.Cast<VideoViewModel>();
            queue.RemoveRange(selectedItems);
        }

        private void ClearAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            queue.Clear();
        }

        private void RenameCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var index = QueueView.SelectedIndex;
            var item = QueueView.SelectedItem as VideoViewModel;
            var renameDialog = new RenameDialog(item);
            renameDialog.Owner = this;
            if (renameDialog.ShowDialog() == true)
                queue.Rename(index, renameDialog.RenamedTitle);
        }

        private void ResetCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            queue.Reset(QueueView.SelectedIndex);
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            queue.Refresh();
        }

        private void StartCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            queue.Save();
        }

        private void QuitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new PresetDialog();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                settings.AddPreset(dialog.Preset.SearchFor, dialog.Preset.ReplaceWith);
                UpdateSettings();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = PresetsList.SelectedIndex;
            var selectedItem = PresetsList.SelectedItem as PresetViewModel;
            var dialog = new PresetDialog(selectedItem);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                settings.UpdatePreset(selectedIndex, dialog.Preset);
                UpdateSettings();
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            settings.MoveUpPreset(PresetsList.SelectedIndex);
            UpdateSettings();
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            settings.MoveDownPreset(PresetsList.SelectedIndex);
            UpdateSettings();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var msgBox = MessageBox.Show("Do you want to delete this preset?",
                "Delete preset", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            var selected = PresetsList.SelectedItem as PresetViewModel;
            if (msgBox == MessageBoxResult.Yes)
            {
                settings.Presets.Remove(selected);
                UpdateSettings();
            }
        }

        private void UpdateSettings()
        {
            settings.Save();
            queue.Refresh();
        }
    }

    public class MainWindowCommands
    {
        public static RoutedUICommand Rename = new(
            "Rename item",
            "Rename",
            typeof(MainWindow),
            new InputGestureCollection() { new KeyGesture(Key.F2) });

        public static RoutedUICommand Reset = new(
            "Reset item",
            "Reset",
            typeof(MainWindow),
            new InputGestureCollection() { new KeyGesture(Key.Back, ModifierKeys.Control) });

        public static RoutedUICommand Refresh = new(
            "Refresh items",
            "Refresh",
            typeof(MainWindow),
            new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });

        public static RoutedUICommand ClearAll = new(
            "Clear all items",
            "ClearAll",
            typeof(MainWindow),
            new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control) });

        public static RoutedUICommand Quit = new(
            "Quit program",
            "Quit",
            typeof(MainWindow),
            new InputGestureCollection() { new KeyGesture(Key.Q, ModifierKeys.Control) });
    }
}
