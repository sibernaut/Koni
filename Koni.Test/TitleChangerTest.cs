// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Koni.Engine.Wrapper;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Xunit;

namespace Koni.Test
{
    public class TitleChangerTest
    {
        static MockFileSystem fileSystem = new();
        Config config = new(fileSystem);

        class TestItem
        {
            public string Path { get; }
            public string Expected { get; }

            public TestItem(string path, string expected)
            {
                Path = path;
                Expected = expected;
            }
        }

        private void AssertEqual(IEnumerable<TestItem> items, VideoFiles videoFiles, Config config, MockFileSystem fileSystem)
        {
            var mockData = new MockFileData("\n");
            var _items = items.Select(x => @"C:\Users\User\Video\Anime\" + x.Path).ToList();
            _items.ForEach(x => fileSystem.AddFile(x, mockData));
            videoFiles.Add(_items);
            var expected = items.Select(x => x.Expected);
            var actual = videoFiles.Items.Select(x => x.Title);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UsingDefaultPresetTest()
        {
            fileSystem.AddDirectory(AppContext.BaseDirectory);
            config.Load();
            var videoFiles = new VideoFiles(config.Presets, fileSystem);
            var items = new List<TestItem>()
            {
                new TestItem(
                    @"Archive\Just Because!\" +
                    "Just Because! - 11.mp4",
                    "Just Because! - 11"
                ),
                new TestItem(
                    @"Archive\Code Geass\Series\Season 2\" +
                    "Code Geass - Hangyaku no Lelouch R2 - 09.mkv",
                    "Code Geass: Hangyaku no Lelouch R2 - 09"
                ),
                new TestItem(
                    @"Archive\Kaguya-sama wa Kokurasetai\OVA\" +
                    "Kaguya-sama wa Kokurasetai - Tensai-tachi no Renai Zunousen OVA.mkv",
                    "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen OVA"
                ),
                new TestItem(
                    @"Music\Kaguya-sama wa Kokurasetai\" +
                    "Kaguya-sama wa Kokurasetai - Tensai-tachi no Renai Zunousen - Opening.mp4",
                    "Kaguya-sama wa Kokurasetai: Tensai-tachi no Renai Zunousen - Opening"
                )
            };

            AssertEqual(items, videoFiles, config, fileSystem);
        }

        [Fact]
        public void UsingCustomPresetsTest()
        {
            fileSystem.AddDirectory(AppContext.BaseDirectory);
            config.Load();
            var presets = config.Presets;
            presets.Add(
                "SK8",
                "SK∞");
            presets.Add(
                "Kumo desu ga, Nani ka",
                "Kumo desu ga, Nani ka?");
            presets.Add(
                "Isekai Maou to Shoukan Shoujo no Dorei Majutsu Omega",
                "Isekai Maou to Shoukan Shoujo no Dorei Majutsu Ω");
            presets.Add(
                "Busou Shoujo Machiavellianism: Doki! 'Goken-darake' no Ian Ryokou",
                "Busou Shoujo Machiavellianism: Doki! \"Goken-darake\" no Ian Ryokou");
            var videoFiles = new VideoFiles(config.Presets, fileSystem);
            var items = new List<TestItem>()
            {
                new TestItem(
                    @"Archive\Isekai Maou to Shoukan Shoujo no Dorei Majutsu\Season 2\" +
                    "Isekai Maou to Shoukan Shoujo no Dorei Majutsu Omega - 03.mkv",
                    "Isekai Maou to Shoukan Shoujo no Dorei Majutsu Ω - 03"
                ),
                new TestItem(
                    @"Archive\Busou Shoujo Machiavellianism\OVA\" +
                    "Busou Shoujo Machiavellianism - Doki! 'Goken-darake' no Ian Ryokou.mp4",
                    "Busou Shoujo Machiavellianism: Doki! \"Goken-darake\" no Ian Ryokou"
                ),
                new TestItem(
                    @"Music\SK8\SK8 - Ending.mp4",
                    "SK∞ - Ending"
                ),
                new TestItem(
                    @"Music\Kumo desu ga, Nani ka\" +
                    "Kumo desu ga, Nani ka - Opening 02.mp4",
                    "Kumo desu ga, Nani ka? - Opening 02"
                )
            };

            AssertEqual(items, videoFiles, config, fileSystem);
        }
    }
}
