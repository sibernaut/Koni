// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Koni.Engine.Wrapper;
using System;
using System.Windows;
using System.Windows.Input;

namespace Koni.WPF
{
    /// <summary>
    /// Interaction logic for PresetsWindow.xaml
    /// </summary>
    public partial class PresetsWindow : Window
    {
        public Config Config;
        Presets presets;
        public PresetsWindow(Config config)
        {
            InitializeComponent();
            Config = config;
            presets = config.Presets;
            PresetsListBox.ItemsSource = presets.Items;
        }

        private void Commands_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            presets.Add("New Search", "New Preset");
            PresetsListBox.SelectedIndex = presets.Items.Count - 1;
            SearchFor.Focus();
            SearchFor.SelectAll();
            ReplaceWith.SelectAll();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (PresetsListBox.SelectedIndex != -1)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            presets.Delete(PresetsListBox.SelectedIndex);
        }

        private void MoveUpCommand_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            if (PresetsListBox.SelectedIndex > 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void MoveUpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            presets.MoveUp(PresetsListBox.SelectedIndex);
        }

        private void MoveDownCommand_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            if (PresetsListBox.SelectedIndex != -1 && PresetsListBox.SelectedIndex < PresetsListBox.Items.Count - 1)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void MoveDownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            presets.MoveDown(PresetsListBox.SelectedIndex);
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.EventArgs e)
        {
            DialogResult = true;
        }
    }

    class PresetsWindowCommands
    {
        public static RoutedUICommand MoveUp = new(
            "Move up item",
            "MoveUp",
            typeof(PresetsWindow),
            new InputGestureCollection() { new KeyGesture(Key.Up, ModifierKeys.Alt) });

        public static RoutedUICommand MoveDown = new(
            "Move down item",
            "MoveDown",
            typeof(PresetsWindow),
            new InputGestureCollection() { new KeyGesture(Key.Down, ModifierKeys.Alt) });
    }
}
