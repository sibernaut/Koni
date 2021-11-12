﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Koni.Engine.Wrapper;
using Microsoft.Win32;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Koni.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Config config = new();
        public VideoFiles queue;
        public MainWindow()
        {
            InitializeComponent();
            config.Load();
            queue = new(config);
            QueueView.ItemsSource = queue.Items;
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
                queue.Add(dropItems);
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
                queue.Add(openFileDialog.FileNames);
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = QueueView.SelectedItems.Cast<VideoFile>().ToList();
            foreach (var item in selectedItems)
                queue.Remove(item);
        }

        private void ClearAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            queue.ClearAll();
        }

        private void RenameCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var index = QueueView.SelectedIndex;
            var item = QueueView.SelectedItem as VideoFile;
            var renameDialog = new RenameDialog(item);
            renameDialog.Owner = this;
            if (renameDialog.ShowDialog() == true)
            {
                var title = renameDialog.RenamedTitle;
                queue.Rename(index, title);
            }
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            config.Load();
            queue.UpdateConfig(config);
            queue.ResetItems();
        }

        private void SettingsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.Show();
        }

        private void StartCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var item in queue.Items)
                item.Save();
        }

        private void QuitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }

    public class MainWindowCommands
    {
        public static RoutedUICommand Rename = new(
            "Rename item",
            "Rename",
            typeof(MainWindow),
            new InputGestureCollection() { new KeyGesture(Key.F2) });

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

        public static RoutedUICommand Settings = new(
            "Open settings",
            "Settings",
            typeof(MainWindow),
            new InputGestureCollection() { new KeyGesture(Key.OemComma, ModifierKeys.Control) });

        public static RoutedUICommand Quit = new(
            "Quit program",
            "Quit",
            typeof(MainWindow),
            new InputGestureCollection() { new KeyGesture(Key.Q, ModifierKeys.Control) });
    }
}
