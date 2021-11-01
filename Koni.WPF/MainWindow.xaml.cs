// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Koni.Engine.Wrapper;
using System;
using System.IO.Abstractions;
using System.Windows;
using System.Windows.Input;

namespace Koni.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Config config = new Config(new FileSystem());
        static Presets presets;
        VideoFiles queue;
        public MainWindow()
        {
            InitializeComponent();
            config.Load();
            presets = config.Presets;
            queue = new(presets, new FileSystem());
            QueueView.ItemsSource = queue.Items;
        }

        public static RoutedUICommand ClearAllCommand = new(
            "Clear All", 
            "ClearAll", 
            typeof(MainWindow), 
            new InputGestureCollection() {
                new KeyGesture(Key.W, ModifierKeys.Control)
            });

        public static RoutedUICommand QuitCommand = new(
            "Quit",
            "Quit",
            typeof(MainWindow),
            new InputGestureCollection() { new KeyGesture(Key.Q, ModifierKeys.Control) });

        private void Commands_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void StartCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var percent = 0;
            foreach (var item in queue.Items)
            {
                item.Save();
                percent += 100 / queue.Items.Count;
                CountStatusBar.Text = percent.ToString();
            }
            CountStatusBar.Text = "All files saved.";
        }

        private void PresetsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var presetsWindow = new PresetsWindow();
            if (presetsWindow.ShowDialog() == true)
            {
                config.Load();
            }
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            queue.Delete(QueueView.SelectedIndex);
        }

        private void SelectAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            QueueView.SelectAll();
        }

        private void QuitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Scroll;
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            Placeholder.Visibility = Visibility.Hidden;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var dropItems = e.Data.GetData(DataFormats.FileDrop) as string[];
                queue.Add(dropItems);
            }
        }

        private void ClearAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            queue.ClearAll();
        }
    }
}
