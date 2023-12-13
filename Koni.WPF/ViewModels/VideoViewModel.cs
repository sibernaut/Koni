using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Koni.WPF.ViewModels
{
    public class VideoViewModel : INotifyPropertyChanged
    {
        private string _title;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Path { get; init; }
        public string FileName { get; init; }
        public string Directory { get; init; }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                IsRenamed = true;
                OnPropertyChanged(nameof(Title));
            }
        }
        public bool IsRenamed { get; set; }
        public bool IsSaved { get; set; }

        public VideoViewModel(string path, SettingsViewModel settings)
        {
            Path = path;
            FileName = System.IO.Path.GetFileName(path);
            Directory = System.IO.Path.GetDirectoryName(path);
            Title = GetTitle(settings);
            IsRenamed = false;
            IsSaved = false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string GetTitle(SettingsViewModel settings)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(Path);
            return settings.ApplyPreset(filename);
        }

        public void Reset(SettingsViewModel settings)
        {
            Title = GetTitle(settings);
            IsRenamed = false;
            IsSaved = false;
            OnPropertyChanged(nameof(Title));
        }

        public void Save()
        {
            if (!IsSaved)
            {
                string ext = System.IO.Path.GetExtension(Path);
                switch (ext)
                {
                    case ".mp4":
                        TagLib.File tfile = TagLib.File.Create(Path);
                        tfile.Tag.Title = Title;
                        tfile.Save();
                        break;
                    case ".mkv":
                        ProcessStartInfo startInfo = new()
                        {
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            FileName = "mkvpropedit.exe",
                            Arguments = String.Format("--edit info --set title=\"{0}\" \"{1}\"", Title, Path)
                        };
                        Process process = Process.Start(startInfo);
                        process.Start();
                        process.WaitForExit();
                        break;
                    default:
                        break;
                }
                IsSaved = true;
                OnPropertyChanged(nameof(IsSaved));
            }
        }
    }
}
