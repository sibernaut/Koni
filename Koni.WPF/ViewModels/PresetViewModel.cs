// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.RegularExpressions;

namespace Koni.WPF.ViewModels
{
    public class PresetViewModel
    {
        public string SearchFor { get; set; }
        public string ReplaceWith { get; set; }

        public PresetViewModel(string searchfor, string replacewith)
        {
            SearchFor = searchfor;
            ReplaceWith = replacewith;
        }

        public static PresetViewModel DefaultPreset =
            new(@"^(.*?)\s-\s(\b(?!Opening|Ending|OVA\b|\d))", "$1: $2");

        public string Apply(string input)
        {
            return Regex.Replace(input, SearchFor, ReplaceWith);
        }
    }
}
