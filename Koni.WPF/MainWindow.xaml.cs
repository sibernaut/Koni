// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Koni.Engine.Wrapper;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace Koni.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Config config = new Config();
        static Presets presets;
        VideoFiles queue;
        public MainWindow()
        {
            InitializeComponent();
            config.Load();
            presets = config.Presets;
            queue = new(presets);
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

        public static RoutedUICommand RenameCommand = new(
            "Rename",
            "Rename",
            typeof(MainWindow),
            new InputGestureCollection() {
                new KeyGesture(Key.E, ModifierKeys.Control),
                new KeyGesture(Key.F2)
            });

        public static RoutedUICommand RefreshCommand = new(
            "Refresh",
            "Refresh",
            typeof(MainWindow),
            new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });

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
                queue.UpdatePresets(config.Presets);
            }
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            queue.Delete(QueueView.SelectedIndex);
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

        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Video files(*.mp4;*.mkv)|*.mp4;*.mkv",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                Placeholder.Visibility = Visibility.Hidden;
                var items = openFileDialog.FileNames;
                queue.Add(items);
            }
        }

        private void RenameCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var index = QueueView.SelectedIndex;
            var item = QueueView.SelectedItem as VideoFile;
            var dialog = new RenameDialog(item);
            if (dialog.ShowDialog() == true)
            {
                var title = dialog.RenamedTitle;
                queue.Rename(index, title);
            }
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            queue.ResetItems();
        }
    }
}
