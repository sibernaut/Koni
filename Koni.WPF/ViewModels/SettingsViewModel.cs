// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Koni.WPF.ViewModels
{
    public class SettingsViewModel
    {
        public ObservableCollection<PresetViewModel> Presets { get; set; }

        public SettingsViewModel()
        {
            Presets = new();
        }

        public void Save()
        {
            JsonSerializerOptions options = new() { WriteIndented = true };
            string jsonStrings = JsonSerializer.Serialize(this, options);
            string path = Path.Combine(AppContext.BaseDirectory, "config.json");
            File.WriteAllText(path, jsonStrings);
        }

        public void Load()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "config.json");
            bool isExist = File.Exists(path);
            if (!isExist)
                Save();
            string jsonStrings = File.ReadAllText(path);
            SettingsViewModel settings = JsonSerializer.Deserialize<SettingsViewModel>(jsonStrings);
            Presets = settings.Presets;
        }

        public string ApplyPreset(string input)
        {
            string output = input;
            Presets.ToList().ForEach(p => output = p.Apply(output));
            return output;
        }

        public void AddPreset(string searchfor, string replacewith)
        {
            var preset = new PresetViewModel(searchfor, replacewith);
            Presets.Add(preset);
        }
        public void RemovePreset(PresetViewModel preset)
        {
            Presets.Remove(preset);
        }
        public void UpdatePreset(int index, PresetViewModel preset)
        {
            Presets[index] = preset;
        }
        public void MoveUpPreset(int index)
        {
            if (index > 0)
                Presets.Move(index, index - 1);
        }
        public void MoveDownPreset(int index)
        {
            if (index < Presets.Count - 1)
                Presets.Move(index, index + 1);
        }
    }
}
