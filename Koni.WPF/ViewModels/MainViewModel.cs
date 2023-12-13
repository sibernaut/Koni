using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Koni.WPF.ViewModels
{
    class MainViewModel
    {
        public ObservableCollection<VideoViewModel> Items { get; init; }
        public SettingsViewModel Settings { get; set; }
        public bool IsBusy { get; set; }

        public MainViewModel()
        {
            Items = new();
            Settings = new();
            IsBusy = false;
            Settings.Load();
        }

        public void AddRange(string[] paths)
        {
            Predicate<string> isFolder = path =>
                System.IO.File.GetAttributes(path).HasFlag(System.IO.FileAttributes.Directory);
            Func<string, bool> isVideo = delegate (string path)
            {
                string ext = System.IO.Path.GetExtension(path);
                return ext is ".mp4" or ".mkv";
            };
            foreach (var path in paths)
            {
                if (isFolder(path))
                    System.IO.Directory.EnumerateFiles(path)
                        .Where(isVideo)
                        .ToList()
                        .ForEach(path => Items.Add(new VideoViewModel(path, Settings)));
                else
                    if (isVideo(path))
                    Items.Add(new VideoViewModel(path, Settings));
            }
        }

        public void Rename(int index, string title)
        {
            Items[index].Title = title;
        }

        public void Reset(int index)
        {
            Items[index].Reset(Settings);
        }

        public void Refresh()
        {
            Items.Where(v => v.IsRenamed is false).ToList().ForEach(v => v.Reset(Settings));
        }

        public void RemoveRange(IEnumerable<VideoViewModel> videos)
        {
            videos.ToList().ForEach(video => Items.Remove(video));
        }

        public void Clear()
        {
            Items.Clear();
        }

        private void Save_DoWork(object sender, DoWorkEventArgs e)
        {
            Items.ToList().ForEach(v => v.Save());
        }

        public void Save()
        {
            BackgroundWorker worker = new();
            worker.DoWork += Save_DoWork;
            worker.RunWorkerAsync();
        }
    }
}
