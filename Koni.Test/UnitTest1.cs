using Koni.Engine.Wrapper;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace Koni.Test
{
    public class UnitTest1
    {
        private class TestItem
        {
            public string FilePath { get; set; }
            public string ExpectedTitle { get; set; }
            public VideoFile VideoFile { get; set; }

            public TestItem(string path, string expected)
            {
                FilePath = path;
                ExpectedTitle = expected;
            }
        }

        private VideoFile GetVideoFile(TestItem item, Presets presets, MockFileSystem mockFileSystem)
        {
            var data = new MockFileData("\n");
            string path = @"C:\Users\User\Video\Anime\" + item.FilePath;
            mockFileSystem.AddFile(path, data);
            return new VideoFile(item.FilePath, presets, mockFileSystem);
        }

        [Fact]
        public void ColonTest()
        {
            var mockFileSystem = new MockFileSystem();
            var config = new Config(mockFileSystem);
            mockFileSystem.AddDirectory(AppContext.BaseDirectory);
            config.Load();
            var presets = config.Presets;
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

            foreach (var item in items)
            {
                item.VideoFile = GetVideoFile(item, presets, mockFileSystem);
                Assert.Equal(item.ExpectedTitle, item.VideoFile.Title);
            }
        }

        [Fact]
        public void PresetsTest()
        {
            var mockFileSystem = new MockFileSystem();
            var config = new Config(mockFileSystem);
            mockFileSystem.AddDirectory(AppContext.BaseDirectory);
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

            foreach (var item in items)
            {
                item.VideoFile = GetVideoFile(item, presets, mockFileSystem);
                Assert.Equal(item.ExpectedTitle, item.VideoFile.Title);
            }
        }
    }
}
